# DoctorPilot — Architecture

> Este documento descreve a implementação arquitetural atualmente observada e, separadamente, as regras arquiteturais desejadas para a solução DoctorPilot.
>
> O código-fonte e os arquivos de projeto são a fonte final de verdade. Caso este documento entre em conflito com a implementação atual, a divergência deve ser investigada antes de novas alterações arquiteturais.

---

## 1. Overview

DoctorPilot is a .NET 10 application organized into multiple projects that separate domain concepts, application use cases, infrastructure, shared contracts, the backend API, and the user interface.

The main solution is located at:

`src/DoctorPilot/DoctorPilot.slnx`

The solution currently contains these projects:

- `DoctorPilot.Core`
- `DoctorPilot.Application`
- `DoctorPilot.Infrastructure`
- `DoctorPilot.Contracts`
- `DoctorPilot.Api`
- `DoctorPilot.Web.Migration`

The backend uses a layered architecture and organizes application use cases with CQRS-style commands, queries, and handlers. The user interface is implemented with ASP.NET Core Razor Components using Interactive Server render mode and MudBlazor.

This document distinguishes three different architectural views:

1. **Compile-time dependencies:** the `ProjectReference` relationships between projects.
2. **Runtime/request flow:** how requests and results move through the running application.
3. **Architectural rules:** desired constraints that guide future changes; they are not, by themselves, statements that every part of the current implementation already complies with them.

---

## 2. Current Project Responsibilities

This section describes the implementation currently observed in the repository. Desired constraints are documented separately under [Architectural Rules](#9-architectural-rules).

### 2.1 DoctorPilot.Core

`DoctorPilot.Core` contains the core domain concepts and persistence abstractions:

- Domain entities
- Enums
- Specialized repository contracts
- Generic `IRepository<T>` contract
- `IUnitOfWork`

It has no project references and does not contain Entity Framework Core, HTTP, API, or UI implementation code.

### 2.2 DoctorPilot.Application

`DoctorPilot.Application` contains application use cases and orchestration logic:

- Commands and queries
- Command and query handlers
- Manual mapping classes
- Application service interfaces such as `ICurrentUserService`, `IPasswordHasher`, and `ITokenGenerator`
- Coordination of repositories and `IUnitOfWork`
- Result construction using contracts from `DoctorPilot.Contracts`

The project uses the `Mediator` library to dispatch commands and queries. Validation is not currently active: there are no registered pipeline behaviors or active application validators.

Existing names include:

```text
InsertAlergiaCommand
InsertAlergiaHandler
GetAlergiaByIdQuery
GetAlergiaByIdHandler
```

### 2.3 DoctorPilot.Infrastructure

`DoctorPilot.Infrastructure` provides technical implementations required by the application:

- Entity Framework Core persistence
- SQL Server database provider
- `DoctorPilotDbContext`
- Entity configurations and EF Core migrations
- Specialized repository implementations
- Generic `Repository<T>` implementation
- `UnitOfWork` implementation
- JWT authentication configuration and token generation
- BCrypt password hashing
- Current-user access through the HTTP context
- Dependency injection registrations for these implementations

Infrastructure implements contracts defined in Core and Application.

### 2.4 DoctorPilot.Contracts

`DoctorPilot.Contracts` currently contains primarily shared response contracts:

- Detail, item, and other response DTOs
- `LoginResponseDto`
- The standard `ResultViewModel<T>` result envelope

It has no project references. It is referenced directly by Application and Web.Migration.

HTTP request bodies are still frequently represented directly by commands from `DoctorPilot.Application` in API controllers. The Web also builds some requests from local form models or anonymous objects. Therefore, dedicated shared request DTOs are not yet the consistent current pattern.

### 2.5 DoctorPilot.Api

`DoctorPilot.Api` is the ASP.NET Core Web API and the application composition root. It is responsible for:

- Application startup
- Registering Application through `AddApplication()`
- Registering Infrastructure through `AddInfrastructure(configuration)`
- Controllers and API endpoints
- Middleware and CORS
- Authentication and authorization pipeline configuration
- HTTP request and response handling
- Dispatching commands and queries through `IMediator`

Controllers currently inject `IMediator`, call `IMediator.Send`, and return the result produced by the application handler. Direct database access was not observed in controllers.

### 2.6 DoctorPilot.Web.Migration

`DoctorPilot.Web.Migration` is an ASP.NET Core Razor Components application using Interactive Server render mode and MudBlazor. It contains:

- Razor components, pages, layouts, forms, and UI state under `Components/`
- Client-side boundary services under `Services/`
- UI-specific form and request models under `Models/`
- Static assets under `wwwroot/`
- A named `HttpClient` and `DoctorPilotApiClient` for communication with the API
- Cookie-based authentication for the Web host

The Web project references only `DoctorPilot.Contracts`. It does not reference API, Application, Core, or Infrastructure and does not access Entity Framework Core or the database directly.

---

## 3. Compile-time Dependencies

The following table records the direct `ProjectReference` relationships currently defined in the `.csproj` files:

| Project | Direct project references |
| --- | --- |
| `DoctorPilot.Core` | None |
| `DoctorPilot.Contracts` | None |
| `DoctorPilot.Application` | `DoctorPilot.Core`, `DoctorPilot.Contracts` |
| `DoctorPilot.Infrastructure` | `DoctorPilot.Application`, `DoctorPilot.Core` |
| `DoctorPilot.Api` | `DoctorPilot.Application`, `DoctorPilot.Infrastructure` |
| `DoctorPilot.Web.Migration` | `DoctorPilot.Contracts` |

Represented as a compile-time dependency graph:

```text
DoctorPilot.Web.Migration ───────────────> DoctorPilot.Contracts

DoctorPilot.Api ──────────> DoctorPilot.Application ──> DoctorPilot.Core
       │                              │
       │                              └───────────────> DoctorPilot.Contracts
       │
       └───────────────> DoctorPilot.Infrastructure ──> DoctorPilot.Application
                                      │
                                      └───────────────> DoctorPilot.Core
```

This graph describes compilation dependencies only. It must not be interpreted as the runtime request flow. For example, Web.Migration calls the API over HTTP at runtime but has no `ProjectReference` to `DoctorPilot.Api`.

The `.csproj` files remain the source of truth for compile-time dependencies.

---

## 4. Runtime and Request Flow

A typical request initiated by the current Web application follows this runtime path:

```text
User
  |
  v
Razor Component (Interactive Server)
  |
  v
DoctorPilotApiClient / HttpClient
  |
  v
HTTP API Endpoint / Controller
  |
  v
IMediator.Send(command or query)
  |
  v
Application Handler
  |
  v
Repository or Service Abstraction
  |
  v
Infrastructure Implementation
  |
  v
Entity Framework Core / SQL Server
```

Results return through the corresponding runtime boundaries:

```text
SQL Server
  |
  v
Infrastructure Repository
  |
  v
Application Handler
  |
  v
ResultViewModel<T> / Shared Response DTO
  |
  v
API HTTP Response
  |
  v
Web Client Service / Razor Component
  |
  v
User
```

Not every use case contains every step. Some handlers also call application service abstractions such as current-user, password-hashing, or token-generation services.

The API is the composition root: at startup it registers Application handlers and binds Core/Application abstractions to Infrastructure implementations. This runtime inversion does not change the compile-time references shown in the previous section.

---

## 5. Current CQRS Implementation

Application use cases are organized using CQRS-style commands and queries implemented with `Mediator` version 3.0.2.

The current implementation uses:

- `ICommand<TResult>` for state-changing requests
- `IQuery<TResult>` for read requests
- `ICommandHandler<TCommand, TResult>` for command handlers
- `IQueryHandler<TQuery, TResult>` for query handlers
- `IMediator.Send` in API controllers for dispatch
- `Mediator.SourceGenerator` for generated mediator registration and dispatch support
- `ResultViewModel<T>` as the standard handler result envelope
- Manually implemented mapping classes rather than an object-mapping library

### Commands

Commands represent operations intended to change application state. Their handlers normally coordinate repositories and call `IUnitOfWork.CommitAsync` to persist changes.

```text
Controller
  |
  v
IMediator.Send(InsertAlergiaCommand)
  |
  v
InsertAlergiaHandler
  |
  +--> Repository abstractions
  |
  └--> IUnitOfWork.CommitAsync
```

### Queries

Queries represent read operations. Their handlers access repository abstractions and map domain results to DTOs from Contracts. Queries do not normally use `IUnitOfWork.CommitAsync`.

```text
Controller
  |
  v
IMediator.Send(GetAlergiaByIdQuery)
  |
  v
GetAlergiaByIdHandler
  |
  v
Repository abstraction
  |
  v
Manual mapping to DTO
```

This is CQRS at the use-case organization and dispatch level. The current implementation does not define separate read and write databases.

There are currently no active mediator pipeline behaviors and no active validation pipeline. Validation is therefore a potential future capability, not part of the observed CQRS implementation.

---

## 6. Current Persistence Pattern

Persistence is implemented in `DoctorPilot.Infrastructure` with:

- Entity Framework Core
- SQL Server
- `DoctorPilotDbContext`
- Specialized repository contracts and implementations
- Generic `IRepository<T>` and `Repository<T>`
- `IUnitOfWork` and `UnitOfWork`
- EF Core entity configurations and migrations

The runtime persistence path is:

```text
Application Handler
  |
  v
Core Repository Contract / IUnitOfWork
  |
  v
Infrastructure Repository / UnitOfWork
  |
  v
DoctorPilotDbContext
  |
  v
Entity Framework Core / SQL Server
```

Detailed database modeling is documented separately in:

`documentos/database.md`

---

## 7. Authentication and Authorization

At a high level, the current authentication boundary is:

- The API uses JWT authentication.
- The Web application uses cookie-based authentication for its own host.

Authentication and authorization span API, Application, Infrastructure, and Web concerns. Their detailed flow, token handling, claims, authorization rules, and configuration belong in:

`documentos/authentication.md`

Authentication-related changes should not be made based only on this general architecture document. Inspect the current implementation and the dedicated authentication documentation first.

---

## 8. Architectural Goals

The architecture aims to:

- Separate business concepts from infrastructure concerns.
- Keep domain and application logic independent from the user interface.
- Keep controllers focused on HTTP/API responsibilities.
- Keep Razor components focused on presentation and user interaction.
- Isolate database and external-service implementations inside Infrastructure.
- Use explicit request and response contracts across application boundaries where appropriate.
- Organize application operations using commands, queries, and handlers.
- Make features easier to test, maintain, and evolve independently.
- Avoid unnecessary coupling between projects.

These are desired architectural goals. They do not convert incidental dependencies or current technical debt into recommended patterns.

---

## 9. Architectural Rules

The rules in this section are desired constraints for future work. They are intentionally separate from the observed implementation described above.

### 9.1 Dependency Direction

- Inner layers should not depend on outer implementation layers.
- Core should not reference Infrastructure, API, or Web.
- Application code should depend on abstractions where appropriate.
- Infrastructure should implement technical abstractions defined by Core or Application.
- The UI should access backend functionality through the established HTTP API and client-service boundaries.
- Before adding or changing a project reference, evaluate its effect on the compile-time dependency graph.

### 9.2 Core

Core should not:

- Contain Entity Framework Core persistence implementation.
- Contain HTTP-specific behavior.
- Contain Razor/Blazor-specific behavior.
- Depend on concrete external-service implementations.

### 9.3 Application and CQRS

- Business and use-case logic should generally be implemented in Core or Application according to its responsibility.
- Inspect similar commands and queries before creating a new use case.
- Preserve existing CQRS naming conventions.
- Keep handlers focused on their use case.
- Avoid duplicating logic across handlers.
- Avoid bypassing Application without a clear architectural reason.
- Handlers should depend on abstractions rather than concrete Infrastructure implementations whenever practical.
- Do not introduce another mediator or use-case pattern when the existing CQRS structure already satisfies the requirement.
- Validation should only be documented as current behavior after it is actually implemented and registered.

### 9.4 Infrastructure and Persistence

- Infrastructure-specific implementation details should remain inside Infrastructure whenever possible.
- Do not move business rules into Infrastructure merely because a use case interacts with the database.
- Entity Framework Core concerns should not leak into Core or UI projects.
- Prefer asynchronous EF Core operations for I/O.
- Avoid unnecessary database queries and N+1 query patterns.
- Use read-only query optimizations where appropriate.
- Do not access `DbContext` directly from Razor components.
- Do not access `DbContext` directly from controllers when an application/repository abstraction should be used.
- Review generated EF Core migrations before applying schema changes.
- Review database impact before destructive migrations are applied.

### 9.5 Contracts

- Contracts should not contain Entity Framework Core configuration.
- Contracts should not depend on persistence implementation details.
- Contracts should not expose database-specific behavior.
- Contracts should not contain application orchestration or UI-specific behavior.
- Persistence entities should not automatically become API contracts.
- Use dedicated request or response types when the external representation differs from the application or domain model.

The current direct use of some Application commands as HTTP request models is an observed implementation detail, not a rule requiring future endpoints to do the same.

### 9.6 API

- Controllers should remain thin and focused on HTTP responsibilities.
- Controllers should receive the HTTP request, dispatch the appropriate application command/query, and return an HTTP response.
- Avoid large controllers containing business workflows.
- Avoid direct database logic in controllers.
- Avoid duplicating application rules in API endpoints.
- Avoid returning persistence-specific implementation details.

### 9.7 Web UI

- Razor components should focus on presentation, interaction, UI state, and calling appropriate services.
- Avoid placing significant business logic directly in components.
- The UI should not become an application-service or persistence layer.
- The UI should not access Entity Framework Core or the database directly.
- Reusable UI behavior should be extracted into components or services when appropriate.

### 9.8 Existing Patterns

Before creating a new abstraction:

- Search the repository for similar functionality.
- Understand the existing implementation.
- Follow the established pattern when appropriate.
- Introduce a new pattern only when there is a clear architectural reason.

Do not treat an accidental dependency or known technical debt as an architectural precedent merely because it exists in the current code.

---

## 10. Architectural Change Guidelines

Changes that affect multiple projects or architectural boundaries require additional care. Examples include:

- Authentication or authorization redesign
- A new persistence strategy
- Changes to CQRS organization
- New external integrations
- Changes to compile-time dependency direction
- Large domain model changes
- Database schema redesign

Before implementing a significant architectural change:

- Inspect the affected projects.
- Identify existing patterns.
- Determine compile-time and runtime dependency impacts separately.
- Determine database impact when applicable.
- Propose the intended approach.
- Implement the smallest coherent change.
- Build the solution.
- Run relevant tests.
- Review the final diff.

Avoid unrelated architectural refactors while implementing a feature.

---

## 11. Related Documentation

Project documentation is stored under `documentos/`.

### Project Specification

`documentos/Especificação do Projeto.md`

Contains functional requirements, intended product behavior, and original/high-level domain modeling. Some technical or modeling sections may be outdated. Do not assume that its diagrams or technical details represent the current implementation without checking the source code.

### Authentication

`documentos/authentication.md`

Should contain the current authentication and authorization architecture in detail.

### Database

`documentos/database.md`

Should contain the current persistence model, important relationships, and database decisions.

---

## 12. Documentation vs. Implementation

Use the following hierarchy when investigating the current implementation:

```text
Current Source Code and Project Files
  |
  v
Current Technical Documentation
  |
  v
Historical / Product Specification
```

The project specification remains important for intended behavior and functional requirements, but it does not automatically override the current technical implementation.

When documentation conflicts with code:

- Inspect the implementation.
- Determine whether the documentation or implementation is outdated.
- Distinguish observed behavior from desired architectural rules.
- Do not silently introduce a third implementation approach.
- Update documentation after the correct architecture has been established.

---

## 13. Source of Truth

The source code and project files are the final source of truth for the current technical implementation.

This document records both observed architecture and desired architectural rules. The two must remain clearly labeled so that an existing accidental dependency is not mistaken for a recommended design.

Before making assumptions about an unfamiliar subsystem:

- Read the relevant documentation.
- Inspect the corresponding source code and project references.
- Find similar existing implementations.
- Follow established patterns unless the requested change explicitly requires modifying them.
