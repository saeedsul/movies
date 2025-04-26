# Introduction

# Project structure.

Movies.sln
.dockerignore
azure-pipeline
Dockerfile
/
|-- Api/
|-- IAC/
|-- Test/

# Api .net 9 project using Repository pattern
# Use Sql server database Server=(localdb)\\mssqllocaldb
# Database is seeded from mymoviedb.csv file that can be downloaded from Url: https://www.kaggle.com/datasets/disham993/9000-movies-dataset
# Serilog for logging , create logs folder in same directory.

# Unit test Project covering scenarios and edge cases


# Dockerfile to build Api image.

# Terraform to create infrastructure for the project located in IAC folder
# in command prompt go to : cd IAC
# terraform init
# terraform plan
# terraform apply --auto-approve
# this will build api project and great the api contianer and the sql db container
# open a browser and go to http://localhost:80 
 
# Azure build Pipeline
# for CI/CD azure-pipeline.yml to test and build project then package it.


# How to run it
# Build solution to restore all packages.

# Api.http file is used for Restful calls to the api. 
# some examples on usage

@Api_HostAddress = http://localhost:5118/api

### 
# Get All Movies (No Filters)
# This request retrieves all movies with default pagination.
# If no query parameters are specified, it returns all movies sorted by the default sort field. 
###
GET {{Api_HostAddress}}/movie/search 
Accept: application/json

###

# Get Movies by Genre
# This request filters movies by a specific genre (e.g., "action"). 
###
GET {{Api_HostAddress}}/movie/search?genre=action
Accept: application/json

###

# Get Movies by Title
# This request searches for movies with a title matching the provided string (e.g., "Spider-Man"). 
###
GET {{Api_HostAddress}}/movie/search?title=Spider-Man
Accept: application/json

###

# Get Movies with Pagination
# This request allows you to search for movies with a specific title and paginate the results.
# The results will be limited to the specified page and number of movies per page. 
###
GET {{Api_HostAddress}}/movie/search?title=Spider-Man&limit=15&page=1
Accept: application/json

###

# Get Movies by Genre (Comedy)
# This request filters movies by the genre "comedy".  
###
GET {{Api_HostAddress}}/movie/search?genre=comedy
Accept: application/json

###

# Get Movies by Release Year
# This request filters movies by their release year. 
###
GET {{Api_HostAddress}}/movie/search?releaseYear=2020
Accept: application/json

###

# Get Movies Sorted by Release Date
# This request retrieves movies sorted by their release date in ascending order. 
###
GET {{Api_HostAddress}}/movie/search?sortby=ReleaseDate
Accept: application/json

###

# Get Movies Sorted by Release Date (Descending)
# This request retrieves movies sorted by their release date in descending order.
###
GET {{Api_HostAddress}}/movie/search?sortby=ReleaseDate&SortOrder=desc
Accept: application/json
