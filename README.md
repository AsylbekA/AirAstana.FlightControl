# AirAstana.FlightControl

Clean Architecture Web API for Air Astana flight status control.

## Project structure
- **Domain** — entities and enums
- **Application** — business logic (CQRS, DTOs, validation)
- **Infrastructure** — EF Core, persistence, caching, logging
- **Api** — Web API (controllers, swagger)
- **Tests** — unit/integration tests

## Run
```bash
dotnet build
dotnet run --project src/AirAstana.FlightControl.Api
```

Swagger: http://localhost:5000/swagger
