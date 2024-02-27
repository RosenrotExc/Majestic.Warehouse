# Use the ASP.NET Core runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Copy the published application to the image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY . .
RUN dotnet restore "src/Majestic.WarehouseService.WebApi/Majestic.WarehouseService.WebApi.csproj"

# Publish the application
RUN dotnet publish "src/Majestic.WarehouseService.WebApi/Majestic.WarehouseService.WebApi.csproj" -c Release -o /app/publish

# Use the base image and copy the published output
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port
EXPOSE 80

# Define the entry point
CMD ["dotnet", "Majestic.WarehouseService.WebApi.dll"]
