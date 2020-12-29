# Build Environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy everything and build
COPY ./src ./src
RUN dotnet publish -c Release ./src/BackendHost/BackendHost.csproj --output ./out

# Build production runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0

# Set Runtime environment variables
ARG ASPNETCORE_ENVIRONMENT
ARG ASPNETCORE_URLS

ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
ENV ASPNETCORE_URLS=${ASPNETCORE_URLS}

# Copy over production dll
WORKDIR /app
COPY --from=build /app/out .

# Set Entrypoint
ENTRYPOINT dotnet BackendHost.dll
