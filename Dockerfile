# Build Environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restore
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Backend.sln ./src/
COPY src/BackendHost/BackendHost.csproj ./src/BackendHost/
RUN dotnet restore ./src
COPY ./src ./src

# Build and Run Development Image
FROM restore as run_dev

ENTRYPOINT ["dotnet", "watch", "--project", "./src/Backend.sln" "run", "--project", ".\BackendHost\BackendHost.csproj"]

# Copy everything else and build
FROM restore as build
RUN dotnet build --no-restore ./src --output ./out

# Build production runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS run_prod
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BackendHost.dll"]
