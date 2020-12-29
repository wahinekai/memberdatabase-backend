#!/usr/bin/env python3

from optparse import OptionParser
from os import system

def parse_command_line_arguments():
    parser = OptionParser()
    parser.add_option("-r", "--restore", action="store_true", dest="restore", default=False, help="Restore project dependencies")
    parser.add_option("-d", "--run-development", action="store_true", dest="run_development", default=False, help="Run dotnet watch run to build/run a hot reloading development version")
    parser.add_option("-b", "--build", action="store_true", dest="build", default=False, help="Run dotnet build to build production Docker Container")
    parser.add_option("-p", "--run-production", action="store_true", dest="run_production", default=False, help="Run Production Docker Container")
    parser.add_option("-s", "--seed-database", action="store_true", dest="seed_database", default=False, help="Seed development database")
    return parser.parse_args()

def dotnetRestore():
    print("Restoring Project Dependencies")
    system("dotnet restore ./src/Backend.sln")

def dotnetBuild():
    print("Building Docker container")
    system("docker build . -t wahinekai/memberdatabase/backend --build-arg ASPNETCORE_ENVIRONMENT=Development --build-arg ASPNETCORE_URLS=http://localhost:5000;https://localhost:5001")

def dotnetRunProduction():
    system("docker run --rm -it -p 5000:5000 -p 5001:5001 wahinekai/memberdatabase/backend")

def dotnetWatchRun():
    print("Running development version")
    system("dotnet watch --project ./src/Backend.sln run --project ./BackendHost/BackendHost.csproj")

def dotnetRunSeedDatabase():
    print("Seeding the database")
    system("dotnet run --project ./src/SeedDatabaseHost/SeedDatabaseHost.csproj")

def main():
    print("Parsing command-line arguments")
    (options, _args) = parse_command_line_arguments()

    if options.restore:
        dotnetRestore()

    if options.build:
        dotnetBuild()

    if options.seed_database:
        dotnetRunSeedDatabase()

    if options.run_development:
        dotnetWatchRun()

    if options.run_production:
        dotnetRunProduction()

if __name__ == "__main__":
    main()