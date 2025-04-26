variable "docker_host" {
  description = "Docker daemon host"
  default = "npipe:////./pipe/docker_engine"
}

variable "network_name" {
  description = "Docker network name"
  default     = "demo_network_movie"
}

variable "db_container_name" {
  description = "SQL Server container name"
  default     = "db_container"
}

variable "db_image_name" {
  description = "SQL Server image name"
  default     = "mcr.microsoft.com/mssql/server:2022-latest"
}

variable "db_password" {
  description = "SA Password for SQL Server"
  default     = "myStong_Password123#"
}

variable "api_container_name" {
  description = "API container name"
  default     = "api_container"
}

variable "api_image_name" {
  description = "API Docker image name"
  default     = "movie:10"
} 

variable "api_dockerfile" {
  description = "Path to API Dockerfile"
  default     = "Api/Dockerfile"
}

variable "db_volume_name" {
  default = "movie_db_data"  
  type = string      
}

 

