#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Source/TrueLayer.Pokemon.Api/TrueLayer.Pokemon.Api.csproj", "Source/TrueLayer.Pokemon.Api/"]
RUN dotnet restore "Source/TrueLayer.Pokemon.Api/TrueLayer.Pokemon.Api.csproj"
COPY . .
WORKDIR "/src/Source/TrueLayer.Pokemon.Api"
RUN dotnet build "TrueLayer.Pokemon.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrueLayer.Pokemon.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrueLayer.Pokemon.Api.dll"]