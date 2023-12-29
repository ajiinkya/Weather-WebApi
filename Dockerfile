#FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
#WORKDIR /app

# Get Full SDK - latest (.NET 6)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everthing else and build
COPY . ./
RUN dotnet publish  -c Release -o out
# Generate runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Weather-WebApi.dll"]