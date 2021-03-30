#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG NUGET_FILE=nuget.config

WORKDIR /src

COPY ./*.sln nuget.config ./
COPY ./Source ./Source
COPY ./Tests ./Tests

RUN dotnet restore --force --configfile $NUGET_FILE

RUN dotnet publish --configuration Release -0 /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

COPY --from=build /publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TrueLayer.Pokemon.Api.dll"]