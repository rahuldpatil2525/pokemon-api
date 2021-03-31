# pokemon-api

## Introduction
This repo contains Pokemon API, Use to get the Pokemon Information and Translation

## Contents

- [.Net 5 Rest API]
- [Docker File] (#docker-file)
- [Test Projects]
  -  `TrueLayer.Pokemon.Api.FunctionalTests` - Functional tests, run the end to end functionality and check all the dependent services run. Here we have mocked only external dependency. So that we can test all the code and each and every use case. Keep Functional test cases coverage as much as possible. 
  - `TrueLayer.Pokemon.Api.UnitTests` - Unit test, this will test unit of work.

## Docker File

This docker file contains the tool needed in the pipeline. the image is based on `mcr.microsoft.com/dotnet/aspnet:5.0`

#### Tools Installed

* .Net 5 framework

### Standard Enviornment Variable 

* ASPNETCORE_ENVIRONMENT - Mandatory to set Enviornment variable to run application, otherwise API will fail to run.

## Build and Test

### Run from Visual Studio

- `Set Profile` - Select Profile from the list of profile. Recommended to use `TrueLayer.Pokemon.Api`. All the required parameters are set in the `launchSetting.json` file.

- Run the Application

- Use `http://localhost:8080/swagger/index.html`url  from browser to test the API using `swagger`

## Contribute