# AbySalto Junior - Restaurant Order API

A REST API for managing restaurant orders, built with ASP.NET Core 9 and Entity Framework Core.

## Tech Stack

- **Framework:** ASP.NET Core 9.0
- **ORM:** Entity Framework Core 9.0 (SQL Server)
- **Validation:** FluentValidation
- **API Docs:** Swagger / OpenAPI

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server

### Setup

1. Clone the repository
2. Update the connection string in `appsettings.Development.json`
3. Apply migrations manually (update-database)
4. Add a few item rows in database
5. Run the project:

```bash
dotnet run --project AbySalto.Junior
```

Swagger UI is available at `/index.html` when running in development.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/restaurant/orders` | Create a new order |
| `GET` | `/api/restaurant/orders` | Get all orders (optional sort by total) |
| `PATCH` | `/api/restaurant/orders/{id}/status` | Update order status |
| `GET` | `/api/restaurant/orders/{id}/total` | Get order total and currency |

## Project Structure

```
AbySalto.Junior/
├── Controllers/       # API endpoints
├── Services/          # Business logic
├── Models/            # Domain entities
├── DTO/               # Request/response models
├── Enums/             # OrderStatus, PaymentType, Currency
├── Validations/       # FluentValidation rules
├── Infrastructure/    # DbContext and database config
└── Migrations/        # EF Core migrations
```
