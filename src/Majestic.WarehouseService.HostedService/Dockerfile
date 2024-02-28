# Use the ASP.NET Core runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .

# Copy the script to the image
COPY src/Majestic.WarehouseService.HostedService/scripts/hostedservice-waitfor-rabbitmq.sh /app/

# Convert line endings to Unix format (LF)
RUN sed -i 's/\r$//' /app/hostedservice-waitfor-rabbitmq.sh

# Install nmap to get ncat
RUN apt-get update && apt-get install -y nmap

# Copy the project file and restore dependencies
RUN dotnet restore "src/Majestic.WarehouseService.HostedService/Majestic.WarehouseService.HostedService.csproj"

# Publish the application
RUN dotnet publish "src/Majestic.WarehouseService.HostedService/Majestic.WarehouseService.HostedService.csproj" -c Release -o /app/publish

# Use the base image and copy the published output
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/hostedservice-waitfor-rabbitmq.sh .

# Grant execute permissions to the script in the final image
RUN chmod +x /app/hostedservice-waitfor-rabbitmq.sh

# Expose the port
EXPOSE 80

# Define the entry point using hostedservice-waitfor-rabbitmq.sh
CMD ["/app/hostedservice-waitfor-rabbitmq.sh", "rabbitmq", "5672", "--", "dotnet", "Majestic.WarehouseService.HostedService.dll"]
