# Build Environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Build environment build args
ARG PACKAGES_TOKEN

# Add Github NuGet Store
RUN dotnet nuget add source https://nuget.pkg.github.com/wahinekai/index.json -n github -u wahinekai -p ${PACKAGES_TOKEN} --store-password-in-clear-text

# Copy project/Solution files and restore
COPY ./src/Backend.sln ./src/Backend.sln
COPY ./src/BackendHost/BackendHost.csproj ./src/BackendHost/BackendHost.csproj
COPY ./src/BackendService/BackendService.csproj ./src/BackendService/BackendService.csproj
RUN dotnet restore ./src/Backend.sln

# Copy everything and build
COPY ./src ./src
RUN dotnet restore ./src/Backend.sln
RUN dotnet build ./src/Backend.sln --no-restore --configuration Release --output ./out

# Build production runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0

# Set Runtime environment variables
ARG ASPNETCORE_ENVIRONMENT

ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}

# Copy over production dll
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

# Set Entrypoint
ENTRYPOINT dotnet BackendHost.dll --urls http://*
