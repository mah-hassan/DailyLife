ًَ# DailyLife API

## Overview

The DailyLife API is a robust web application developed using ASP.NET Core with .NET 8, following the Clean Architecture principles combined with Domain-Driven Design (DDD) and Command Query Responsibility Segregation (CQRS) patterns. The application utilizes MediatR for managing requests and notifications, Entity Framework Core for data access, and Quartz for scheduling tasks. The project is structured to support two separate databases: one for identity management (using `AppIdentityDbContext`) and another for business logic and other concerns (using `BusinessDbContext`).

## Project Architecture

### Clean Architecture with DDD and CQRS
- **Clean Architecture**: The project is organized into layers to achieve separation of concerns. This includes layers such as Presentation, Application, Domain, and Infrastructure.
- **Domain-Driven Design (DDD)**: The core of the application is built around the domain and its logic, which ensures that the software reflects the business domain accurately.
- **CQRS**: Command Query Responsibility Segregation (CQRS) is used to separate read and write operations, enhancing performance and scalability.

### Technologies and Packages
- **ASP.NET Core**: The primary framework for building the web API.
- **MediatR**: Used for handling requests and notifications.
- **Entity Framework Core (EF Core)**: ORM for data access.
- **Newtonsoft.Json**: Library for JSON serialization and deserialization.
- **Quartz**: Scheduling library for background tasks.
- **Otp.Net**: Library for generating and validating OTP (One Time Password).

### Databases 
- **AppIdentityDbContext**: Handles all identity-related data operations.
- **BusinessDbContext**: Manages business-related data and operations.

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/your-repo/dailylife-api.git
    cd dailylife-api
    ```

2. Restore dependencies:
    ```bash
    dotnet restore
    ```

3. Update database connections in `appsettings.json`.

4. Apply database migrations:
    ```bash
    dotnet ef database update --context AppIdentityDbContext
    dotnet ef database update --context BusinessDbContext
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

### Testing the API
Use the provided Postman collection (`DailyLife.postman_collection.json`) to test the API endpoints. Import the collection into Postman and set the `baseurl` environment variable to the running instance of the API (e.g., `https://localhost:5001`).Also the apis are deployed on monster asp on https://dailylife.runasp.net
