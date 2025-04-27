
terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0.1"
    }
  }
}

provider "docker" {
  host = var.docker_host
} 

output "build_details" {
  value = {
    dockerfile_path = "${path.module}/../DatabaseSeeder/Dockerfile" 
    dockerfile_exists = fileexists("${path.module}/../DatabaseSeeder/Dockerfile")
  }
}

# Define Database seeder image
resource "docker_image" "db_seeder_image" {
  name = "db-seeder:10"
  build {
    context    = abspath("${path.module}/../DatabaseSeeder/")   
    dockerfile = "Dockerfile"  
  }
}

# Define React app image
resource "docker_image" "react_app" {
  name = var.react_image_name
  build {
    context    = "${path.module}/../ux/"   
    dockerfile = "Dockerfile"   
  }   
} 
 

# Define Docker api image
resource "docker_image" "api_image" {
  name = var.api_image_name
  build {
    context    = abspath("${path.module}/../")   
    dockerfile = "Dockerfile"  
  }
}
 
 
# Define Docker network
resource "docker_network" "app_network" {
  name = var.network_name
  depends_on   = [docker_image.api_image]
}

# Define persistent volume for SQL Server data
resource "docker_volume" "sqlserver_data" {
  name = var.db_volume_name
  depends_on   = [docker_image.api_image]
}

# Define SQL Server container with persistent storage
resource "docker_container" "sql_server_db" {
  name         = var.db_container_name
  image        = var.db_image_name
  ports {
    internal = 1433
    external = 8002
  }
  env = [
    "ACCEPT_EULA=Y",
    "MSSQL_SA_PASSWORD=myStong_Password123#"
  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_image.api_image]
  # Mount the persistent volume for SQL Server data
  mounts {
    target = "/var/opt/mssql"
    source = docker_volume.sqlserver_data.name
    type   = "volume"
  }
}

# Define API container
resource "docker_container" "api_container" {
  name         = var.api_container_name
  image        = docker_image.api_image.name
  ports {
    internal = 80
    external = 8001
  }
  env = [
    "ConnectionStrings__dbConnectionString=Server=${docker_container.sql_server_db.name};Database=SqlDemoDb;User Id=SA;Password=myStong_Password123#;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
    "ASPNETCORE_ENVIRONMENT=Development",
    "ASPNETCORE_URLS=http://+:80"
  ]
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.sql_server_db, docker_image.api_image]
  
  # Define volumes for API container
  volumes {
    host_path      = "${pathexpand("~")}/.microsoft/usersecrets"
    container_path = "/root/.microsoft/usersecrets"
    read_only      = true
  }

  volumes {
    host_path      = "${pathexpand("~")}/.aspnet/https"
    container_path = "/root/.aspnet/https"
    read_only      = true
  }
  healthcheck {
    test     = ["CMD", "curl", "-f", "http://localhost:80/health"]
    interval = "30s"
    timeout  = "10s"
    retries  = 3
  }
} 
  
# Database Seeder container
resource "docker_container" "container_seeder" {
  name         = "container_seeder"
  image        = docker_image.db_seeder_image.name
  env = [
    "DB_CONNECTION_STRING=Server=db_container,1433;Database=SqlDemoDb;User Id=SA;Password=myStong_Password123#;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
  ]
  network_mode = docker_network.app_network.name   
  depends_on   = [docker_container.sql_server_db]  
 
  must_run     = true
  restart      = "no"
}


# React App Container
resource "docker_container" "react_app_container" {
  name         = var.react_container_name
  image        = docker_image.react_app.name   

  ports {
    internal = 80
    external = 3000
  }

  networks_advanced {
    name         = docker_network.app_network.name
    aliases      = ["react-frontend"]
  }
  
  network_mode = docker_network.app_network.name
  depends_on   = [docker_container.api_container, docker_image.react_app]

  env = [
    "VITE_API_URL=http://localhost:8001",
    "VITE_API_INTERNAL_URL=http://${docker_container.api_container.name}:80",
    "VITE_API_LOCAL=http://localhost:5118"
  ]

   healthcheck {
    test     = ["CMD", "curl", "-f", "http://localhost:80"]
    interval = "30s"
    timeout  = "10s"
    retries  = 3
  }

  labels {
    label = "traefik.enable"
    value = "true"
  }

  labels {
    label = "traefik.http.routers.react.rule"
    value = "Host(`react.localhost`)"
  }

  volumes {
    container_path = "/app/node_modules"
    volume_name    = "react_node_modules"
    read_only      = false
  }

  volumes {
    host_path      = abspath("${path.module}/../ux/src")
    container_path = "/app/src"
    read_only      = true
  }

  restart = "unless-stopped"
  must_run = true
}

