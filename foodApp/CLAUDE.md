# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Explorer is a modular .NET 7 web API backend built with a clean architecture pattern. It follows a multi-module architecture where each module (Stakeholders, Blog, Tours) has a Core, API, and Infrastructure layer. The API is organized around business domains rather than technical concerns.

### Key Tech Stack
- **Framework**: ASP.NET Core 7
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Testing**: xUnit, Shouldly, ArchUnitNET
- **Dependency Injection**: Microsoft Extensions
- **Mapping**: AutoMapper
- **API Documentation**: Swagger/Swashbuckle

## Solution Structure

```
src/
├── Explorer.sln                          # Main solution file
├── Explorer.API/                         # Web API entry point
│   ├── Program.cs                        # Startup configuration
│   ├── Startup/                          # Configuration classes
│   │   ├── AuthConfiguration.cs          # JWT authentication setup
│   │   ├── SwaggerConfiguration.cs       # API documentation
│   │   ├── CorsConfiguration.cs          # CORS policy setup
│   │   └── ModulesConfiguration.cs       # Module registration
│   ├── Controllers/                      # API endpoints (organized by role)
│   ├── appsettings.json                  # Configuration
│   └── Properties/launchSettings.json    # IIS Express launch config
├── BuildingBlocks/                       # Shared infrastructure
│   ├── Explorer.BuildingBlocks.Core/     # Base classes and interfaces
│   │   ├── Domain/Entity.cs              # Base entity class
│   │   └── UseCases/                     # BaseService, ICrudRepository, helpers
│   ├── Explorer.BuildingBlocks.Infrastructure/  # Database abstractions
│   │   └── Database/CrudDatabaseRepository.cs   # Generic CRUD implementation
│   └── Explorer.BuildingBlocks.Tests/    # Test base classes and fixtures
├── Modules/                              # Feature modules
│   ├── Stakeholders/                     # User, restaurant, food, orders (currently wired)
│   │   ├── Explorer.Stakeholders.Core/   # Domain + services
│   │   ├── Explorer.Stakeholders.API/    # DTOs + public interfaces
│   │   └── Explorer.Stakeholders.Infrastructure/  # DB context + repositories
│   ├── Blog/                             # Not yet integrated
│   └── Tours/                            # Not yet integrated
└── Explorer.Architecture.Tests/          # Enforces architectural rules
```

## Module Architecture Pattern

Each module follows a **layered clean architecture** with strict dependency rules (enforced by ArchUnitNET):

### Layer 1: API Layer (`*.API`)
- **Responsibility**: DTOs, public interfaces, service contracts
- **Dependencies**: Only BuildingBlocks.Core
- **Example**: `Explorer.Stakeholders.API/` contains `IAuthenticationService`, `IRestaurantService`, etc.

### Layer 2: Core Layer (`*.Core`)
- **Responsibility**: Domain entities, business logic, use cases
- **Dependencies**: BuildingBlocks.Core, own API project
- **Structure**:
  - `Domain/` - Entities (inherit from `Entity` base class), domain logic, repository interfaces
  - `UseCases/` - Service implementations
  - `Mappers/` - AutoMapper profiles for DTO/Domain mapping
- **Example**: `Explorer.Stakeholders.Core/Domain/User.cs` with role-based access control

### Layer 3: Infrastructure Layer (`*.Infrastructure`)
- **Responsibility**: Database access, external service integration, configuration
- **Dependencies**: BuildingBlocks.Infrastructure, own Core/API projects
- **Structure**:
  - `Database/` - DbContext, migrations, repository implementations
  - `Authentication/` - JWT token generation, auth service implementation
  - `*Startup.cs` - Dependency injection registration (called from main Program.cs)
- **Example**: `StakeholdersStartup.ConfigureStakeholdersModule()` registers all services via DI

## Database Schema

The application uses PostgreSQL with schema-based table organization:
- **stakeholders** schema: User, Person, Restaurant, Food, Order, RestaurantRating, RatingReport tables
- Each module gets its own schema when integrated
- **Key**: DbContext uses Npgsql with specific schema configuration in migrations history

## Authentication & Authorization

- **JWT Strategy**: `AuthConfiguration.cs` configures symmetric key signing with configurable issuer/audience
- **Environment Variables**:
  - `JWT_KEY` (default: "explorer_secret_key")
  - `JWT_ISSUER` (default: "explorer")
  - `JWT_AUDIENCE` (default: "explorer-front.com")
- **Roles**: Three authorization policies defined:
  - `administratorPolicy`
  - `authorPolicy`
  - `touristPolicy`
- **Token Expiration**: Tracked via custom response header "AuthenticationTokens-Expired"

## Testing Strategy

### Test Projects
- `Explorer.BuildingBlocks.Tests` - Base fixtures and integration test helpers
- `Explorer.Stakeholders.Tests` - Integration tests (uses xUnit + Shouldly)
- `Explorer.Architecture.Tests` - Enforces module dependency rules using ArchUnitNET

### Test Infrastructure
- **BaseTestFactory**: Manages WebApplicationFactory, in-memory database, test data seeding
  - Looks for SQL scripts in `TestData/` folder for database initialization
  - Handles multiple DbContext initialization with exception handling
- **BaseWebIntegrationTest**: Provides HTTP client and claim principal builders
  - `BuildContext(id)` creates controller context with personId claim for authorization testing

### Running Tests
```bash
# Build and run all tests
dotnet build
dotnet test

# Run specific test project
dotnet test src/Modules/Stakeholders/Explorer.Stakeholders.Tests/Explorer.Stakeholders.Tests.csproj

# Run with coverage
dotnet test /p:CollectCoverage=true

# Architecture tests (enforces layer rules)
dotnet test src/Explorer.Architecture.Tests/Explorer.Architecture.Tests.csproj
```

## Building & Running

### Prerequisites
- .NET 7 SDK
- PostgreSQL 12+ (for local development)

### Build
```bash
dotnet build src/Explorer.sln
```

### Run the API
```bash
# Development mode (Swagger at /swagger)
cd src/Explorer.API
dotnet run

# Production mode
dotnet run --configuration Release
```

The API listens on:
- HTTP: `http://localhost:5000` (or configured port)
- HTTPS: `https://localhost:5001`
- IIS Express: `http://localhost:51713` (see launchSettings.json)
- Swagger UI: `http://localhost:{port}/swagger`

### Configuration
- **Database**: Connection string built via `DbConnectionStringBuilder.Build("stakeholders")`
- **CORS**: Default allows `http://localhost:4200`; configure via `EXPLORER_CORS_ORIGINS` environment variable pointing to a file with allowed origins
- **Development**: Uses `appsettings.Development.json` when `ASPNETCORE_ENVIRONMENT=Development`

## API Error Handling

The `BaseApiController` provides standardized error responses:
- Errors use `FluentResults` library with metadata-based status codes
- Status code determined by error metadata `code` field (400, 403, 404, 409, default 500)
- Subcode metadata appended to error messages for detailed diagnostics
- Development environment shows detailed exception page; production uses `/error` endpoint

## Key Design Patterns

### 1. Generic CRUD Repository Pattern
- `ICrudRepository<TEntity>` interface in BuildingBlocks.Core
- `CrudDatabaseRepository<TEntity, TDbContext>` provides generic implementation
- Modules override for specialized queries (e.g., `RestaurantRepository` extends base)
- Get, Create, Update, Delete, GetPaged(int page, int pageSize) methods

### 2. Service Layer with Mapping
- `BaseService<TDto, TDomain>` provides AutoMapper injection and mapping helpers
- Services return `Result<T>` from FluentResults for rich error handling
- Automatic DTO↔Domain mapping with `MapToDto()` and `MapToDomain()`
- PagedResult support for list endpoints

### 3. Dependency Injection Registration
- Each module's `*Startup.cs` class registers its services (example: `StakeholdersStartup`)
- `SetupCore()` - service interfaces bound to implementations
- `SetupInfrastructure()` - repository and DbContext registration
- Called from `ModulesConfiguration.RegisterModules()` in main Program.cs

### 4. AutoMapper Profiles
- Each module has a mapper profile (e.g., `SteholderProfile`) for DTO/Domain mapping
- Registered globally: `AddAutoMapper(typeof(ModuleProfile).Assembly)`

## Adding a New Module

1. Create three projects following the Stakeholders pattern:
   - `Explorer.{Module}.API` → DTOs, interfaces
   - `Explorer.{Module}.Core` → Domain, services, mappers
   - `Explorer.{Module}.Infrastructure` → DbContext, repositories, *Startup.cs

2. Create `{Module}Startup.cs` with `Configure{Module}Module()` extension method:
   ```csharp
   public static IServiceCollection Configure{Module}Module(this IServiceCollection services)
   {
       services.AddAutoMapper(typeof(ModuleProfile).Assembly);
       // ... register services and repositories
       services.AddDbContext<ModuleContext>(...);
       return services;
   }
   ```

3. Call from `ModulesConfiguration.RegisterModules()`:
   ```csharp
   services.Configure{Module}Module();
   ```

4. Add projects to Explorer.sln and configure as nested in solution structure

5. Create architectural tests in `Explorer.Architecture.Tests` referencing the new module

## Common Tasks

### Add a New Entity
1. Create domain class in `Core/Domain/` inheriting from `Entity`
2. Add repository interface in `Domain/RepositoryInterfaces/`
3. Implement in `Infrastructure/Database/Repositories/`
4. Create DTO in `API/Dtos/`
5. Create AutoMapper profile in `Core/Mappers/`
6. Register repository in module's *Startup.cs

### Add an API Endpoint
1. Create or extend controller in `API/` inheriting from `BaseApiController`
2. Inject service into constructor
3. Map action result using `CreateResponse<T>(Result<T> result)`
4. Service handles business logic, returns `Result<T>`

### Add a Database Migration
```bash
cd src/Modules/{Module}/Explorer.{Module}.Infrastructure
dotnet ef migrations add {MigrationName} -c {DbContextName}
dotnet ef database update
```

### Add Tests
- Integration tests in `{Module}.Tests/` using `BaseWebIntegrationTest<TestFactory>`
- Test data: Add SQL scripts to `TestData/` folder (processed in alphabetical order during BaseTestFactory initialization)
- Architecture tests in `Explorer.Architecture.Tests/` using ArchUnitNET

## Important Files

- **Entry Point**: `src/Explorer.API/Program.cs` - Startup configuration, middleware setup
- **Architectural Rules**: `src/Explorer.Architecture.Tests/ModulesTests.cs` - Enforced layer dependencies
- **Base Classes**: 
  - `BuildingBlocks.Core/UseCases/BaseService.cs`
  - `BuildingBlocks.Infrastructure/Database/CrudDatabaseRepository.cs`
- **Test Fixtures**: `BuildingBlocks.Tests/BaseTestFactory.cs`, `BaseWebIntegrationTest.cs`

## Notes for Future Work

- **Blog** and **Tours** modules are created but not integrated into the main API (see Explorer.sln)
- The solution is designed for horizontal scalability by module; each module can be deployed independently once fully integrated
- Architectural tests ensure modules don't violate layering rules—run before commits
- The codebase uses explicit nullable reference types (`<Nullable>enable</Nullable>`), so be precise with null safety
