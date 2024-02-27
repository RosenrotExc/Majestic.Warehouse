# Majestic.WarehouseService

## Overview

This project is a simple multilayer microservice designed for demonstration purposes. It features a single controller with four CRUD endpoints and an additional endpoint for asynchronously processing "Car Sell" operations, handled by a separate executable hosted service.

## Features

- **Controller with CRUD Endpoints**: The microservice includes a controller exposing four CRUD endpoints for managing essential operations.

- **Asynchronous Car Sell Operation**: A dedicated endpoint facilitates the asynchronous processing of "Car Sell" operations. This is achieved through a separate executable hosted service.

- **Audit Log**: The project incorporates a comprehensive audit log system. Every change and action within the database is meticulously logged, providing a detailed history of modifications. This audit log is a valuable tool for tracking and understanding alterations made to the system, enhancing transparency and accountability.

## Architecture

The project is structured as a multilayer application with distinct layers, including:

- **WebApi**: This layer contains the controller responsible for handling incoming HTTP requests, exposing CRUD endpoints, and managing the overall API functionality.

- **Services**: The services layer encapsulates business logic and application-specific operations, implementing the CQRS (Command Query Responsibility Segregation) approach for a clear separation of command and query responsibilities.

- **Repository**: Responsible for data access and interaction with the database, the repository layer ensures efficient storage and retrieval of information.

- **Models**: This layer defines the data models used throughout the application, ensuring a standardized representation of entities.

The use of the CQRS pattern enhances flexibility and maintainability, allowing for scalable and well-organized development.

## Usage

1. **Clone the repository:**

   ```bash
    git clone https://github.com/RosenrotExc/Majestic.Warehouse.git

2. **Navigate to the project directory:**

   ```bash
    cd Majestic.Warehouse

2. **Build and run the microservice:**

   ```bash
    dotnet build
    dotnet run

## Dependencies

.NET 6 - Ensure you have .NET 6 installed on your machine.

## Configuration

Adjust the configuration settings in appsettings.json and launchSettings.json to suit your environment and preferences.
