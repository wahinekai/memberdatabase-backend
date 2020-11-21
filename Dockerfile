# Build Environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy everything and build
COPY ./src ./src
RUN dotnet publish -c Release ./src/BackendHost/BackendHost.csproj --output ./out

# Build production runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0

# Set Runtime environment variables
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV ASPNETCORE_URLS="http://0.0.0.0:5000;https://0.0.0.0:5001"

# Expose Ports
EXPOSE 5000 
EXPOSE 5001

# Copy over production dll
WORKDIR /app
COPY --from=build /app/out .

# Set Entrypoint
ENTRYPOINT dotnet BackendHost.dll
