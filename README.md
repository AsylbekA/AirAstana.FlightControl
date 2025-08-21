# AirAstana.FlightControl

Clean Architecture Web API for Air Astana flight status control.

## Project structure
- **Domain** — entities and enums
- **Application** — business logic (CQRS, DTOs, validation)
- **Infrastructure** — EF Core, persistence, caching, logging
- **Api** — Web API (controllers, swagger)
- **Tests** — unit/integration tests


## DB Posgersql
Команды EF
- **создать миграцию:** dotnet ef migrations add InitialCreate \
  -p src/AirAstana.FlightControl.Infrastructure \
  -s src/AirAstana.FlightControl.Api

- **обновить базу: ** dotnet ef database update \
-p src/AirAstana.FlightControl.Infrastructure \
-s src/AirAstana.FlightControl.Api

## Run
```bash
dotnet build
dotnet run --project src/AirAstana.FlightControl.Api
```

Swagger: http://localhost:5000/swagger
