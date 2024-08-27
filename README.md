# Public Space Maintenance Request Management System - Backend API

This repository contains the backend API for the Public Space Maintenance Request Management System, built with ASP.NET Core. 
<br> The backend serves as the core server-side application that manages public space maintenance requests submitted by citizens and processed by municipal officers.

## Features

- **Role-Based Access Control:** Different roles including Citizen, Officer, and Admin, each with specific permissions.
- **Request Management:** Citizens can submit requests for public space issues (e.g., cleanliness, greenery), which officers can then review and manage.
- **Authentication and Authorization:** Secure authentication and authorization using ASP.NET Identity.
- **RESTful API:** Well-defined RESTful endpoints for managing requests and user roles.
- **Data Persistence:** Entity Framework Core used for data persistence with a SQL Server database.
- **Logging and Error Handling:** Centralized logging and error handling to ensure robustness and maintainability.

## Technologies Used

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **SQL Server** (for data storage and management)
- **ASP.NET Identity**
- **Automapper** (for object mapping)
- **Serilog** (for logging)
- **Swagger** (for API documentation)

## Getting Started

### Prerequisites

- .NET 8.0 SDK or higher
- SQL Server (local or cloud-based)

### Setup and Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/psm-request-backend.git
   cd psm-request-backend```

2. Restore the dependencies and tools required for the project:
   ```dotnet restore```

3. Update the appsettings.json file with your SQL Server connection string and other necessary configurations.

4. Apply database migrations to set up the SQL Server database:
   ```dotnet ef database update```

5. Run the applicaton
   ```dotnet run```
The API will be available at http://localhost:5000/api.

### API Documentation
The API documentation is available via Swagger. Navigate to http://localhost:5000/swagger once the application is running.

### Development
#### Code Structure
- **Controllers**: API controllers handling incoming HTTP requests.
- **Models**: Data models representing the entities in the system.
- **Services**: Business logic and operations related to request handling.
- **Data**: Database context and migration configurations.
- **DTOs**: Data Transfer Objects used for transferring data between the client and server

### Running Tests
- Unit tests can be executed using the following command:
  ```dotnet test```
