# Portfolio Platform — Clean Architecture

ASP.NET Core 9 solution aligned with the existing SQL Server `Portfolio` database and Dapper stored-procedure layer.

## Solution layout

```
Portfolio/
├── Portfolio.sln
├── ARCHITECTURE.md
└── src/
    ├── Portfolio.Api/                 # Presentation (HTTP host)
    ├── Portfolio.Application/         # Use cases, contracts, DTOs
    ├── Portfolio.Domain/              # Entities, enums, domain rules
    ├── Portfolio.Infrastructure/      # JWT, Serilog host wiring, cross-cutting
    └── Portfolio.Persistence/         # Dapper + SQL Server repositories
```

## Project references (dependency rule)

```
Portfolio.Api
    ├── Portfolio.Application
    └── Portfolio.Infrastructure

Portfolio.Infrastructure
    ├── Portfolio.Application
    └── Portfolio.Persistence

Portfolio.Persistence
    └── Portfolio.Application

Portfolio.Application
    └── Portfolio.Domain

Portfolio.Domain
    └── (none)
```

**Rule:** dependencies point inward only. `Domain` has zero project references.

## Dependency flow

```mermaid
flowchart TB
    subgraph Presentation["Presentation Layer"]
        API["Portfolio.Api<br/>Program.cs, OpenAPI, Serilog host"]
    end

    subgraph Infrastructure["Infrastructure Layer"]
        INF["Portfolio.Infrastructure<br/>JWT, CurrentUser, DI composition"]
    end

    subgraph Application["Application Layer"]
        APP["Portfolio.Application<br/>Repository interfaces, models, settings"]
    end

    subgraph Persistence["Persistence Layer"]
        PER["Portfolio.Persistence<br/>Dapper, SqlConnection, repositories"]
    end

    subgraph Domain["Domain Layer"]
        DOM["Portfolio.Domain<br/>Entities, exceptions, enums"]
    end

    DB[(SQL Server<br/>Portfolio DB)]

    API --> APP
    API --> INF
    INF --> APP
    INF --> PER
    PER --> APP
    APP --> DOM
    PER --> DB
```

## Request flow (future controllers)

```mermaid
sequenceDiagram
    participant Client
    participant Api as Portfolio.Api
    participant App as Application Service
    participant Repo as Persistence Repository
    participant SP as SQL Stored Procedure

    Client->>Api: HTTP Request + JWT
    Api->>Api: Authentication / Authorization
    Api->>App: Invoke use case
    App->>Repo: IExperienceRepository (etc.)
    Repo->>SP: Dapper Execute / Query
    SP-->>Repo: Rows + @StatusCode OUTPUT
    Repo-->>App: Entity / PagedResult
    App-->>Api: Response DTO
    Api-->>Client: JSON
```

## Folder structure by project

### Portfolio.Domain
```
Common/           AuditableEntity, ISoftDeletable
Entities/         Experience, Projects, Education, Skills, Awards, Certifications, Documents
Enums/            SpStatusCode
Exceptions/       DomainException, NotFoundException
```

### Portfolio.Application
```
Abstractions/
  Identity/       IJwtTokenService
  Persistence/    I*Repository, IDbConnectionFactory
Common/
  Interfaces/     ICurrentUserService, IDateTimeProvider
  Models/         PagedResult, SpExecutionResult, PaginationRequest
  Settings/       DatabaseSettings
Features/         (per-module use cases — add next)
DependencyInjection.cs
```

### Portfolio.Persistence
```
Connection/       SqlConnectionFactory
Common/           StoredProcedureExecutor
Repositories/     Experience, Projects, Education, Skills, Awards, Certifications, Documents
DependencyInjection.cs
```

### Portfolio.Infrastructure
```
Authentication/   JwtSettings, JwtTokenService
Services/         CurrentUserService, DateTimeProvider
DependencyInjection.cs
```

### Portfolio.Api
```
Extensions/       SerilogExtensions
Program.cs
appsettings.json
Controllers/      (add later — not scaffolded)
```

## Technology mapping

| Concern | Implementation |
|---------|----------------|
| Runtime | ASP.NET Core 9 |
| Data access | Dapper → existing `usp_*` stored procedures |
| Database | SQL Server (`Portfolio`) |
| Auth | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Logging | Serilog (Console + rolling file) |
| DI | Built-in `IServiceCollection` extension methods |

## Deployment order (database already exists)

1. Run `Database/00_Deploy_All.sql` if schema/SPs are not applied.
2. Update `appsettings.json` → `Database:ConnectionString`.
3. Replace `Jwt:SecretKey` with a secure value (User Secrets in dev).
4. `dotnet build` then `dotnet run --project src/Portfolio.Api`.

## Next steps

1. Add `Features/{Module}/` commands & queries in Application.
2. Implement Dapper calls in Persistence repositories.
3. Add API controllers (thin — delegate to Application only).
4. Add global exception middleware and `ProblemDetails` mapping for `SpStatusCode`.
