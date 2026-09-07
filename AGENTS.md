# Repository Guidelines

## Project Structure & Module Organization

The .NET 10 solution is under `src/DoctorPilot/DoctorPilot.slnx`. Backend code is split by responsibility: `DoctorPilot.Api` contains controllers and startup configuration; `DoctorPilot.Application` contains CQRS commands, queries, mappings, and service interfaces; `DoctorPilot.Core` owns entities, enums, and repository contracts; and `DoctorPilot.Infrastructure` implements persistence, EF Core migrations, repositories, authentication, and other services. Shared request/response types belong in `DoctorPilot.Contracts`. The Blazor Server UI is in `DoctorPilot.Web.Migration`, with pages and components under `Components/`, client services under `Services/`, and static assets under `wwwroot/`. Project documentation lives in `documentos/`.

## Build, Test, and Development Commands

Run commands from the repository root:

- `dotnet restore src/DoctorPilot/DoctorPilot.slnx` restores NuGet packages.
- `dotnet build src/DoctorPilot/DoctorPilot.slnx` compiles all projects and reports analyzer/compiler errors.
- `dotnet run --project src/DoctorPilot/api/DoctorPilot.Api` starts the API.
- `dotnet run --project src/DoctorPilot/DoctorPilot.Web.Migration` starts the Blazor UI.
- `dotnet test src/DoctorPilot/DoctorPilot.slnx` runs all test projects once they are added.
- `dotnet format src/DoctorPilot/DoctorPilot.slnx --verify-no-changes` checks standard .NET formatting before review.

## Coding Style & Naming Conventions

Use four-space indentation, file-scoped or existing namespace style consistently, nullable reference types, and implicit usings as configured by the projects. Follow standard C# naming: PascalCase for types, methods, properties, and public members; camelCase for parameters and locals; `I` prefixes for interfaces. Keep one primary type per file and match its filename. Preserve feature-oriented CQRS names such as `InsertAlergiaCommand`, `InsertAlergiaHandler`, and `GetAlergiaByIdQuery`.

## Testing Guidelines

There is currently no committed test project or coverage threshold. Add tests in sibling projects named `DoctorPilot.<Layer>.Tests`, using files such as `LoginHandlerTests.cs`. Prefer unit tests for handlers, mappings, and domain rules; use integration tests for controllers, authorization, and EF Core repositories. Every bug fix should include a regression test when practical.

## Commit & Pull Request Guidelines

Recent history uses Conventional Commit-style prefixes, primarily `feat:`. Continue with concise, imperative subjects such as `feat: add appointment validation` or `fix: restrict clinic data access`. Keep commits focused. Pull requests should explain the behavior change, identify affected layers, link relevant issues, list validation commands, and include screenshots for Blazor UI changes. Call out migrations or configuration changes explicitly.

## Security & Configuration

Never commit secrets, JWT keys, connection strings, `.env` files, or user-specific `*.user` settings. Keep clinic scoping and authorization checks intact, and document any required local values for `appsettings.Development.json` in the pull request without exposing credentials.

## Architecture Rules

Respect the existing project boundaries and dependency direction.

- `DoctorPilot.Core` contains domain entities, enums, and abstractions and must not depend on Infrastructure, API, or Web.
- `DoctorPilot.Application` contains use cases, CQRS handlers, mappings, and application abstractions. Keep business/use-case logic here rather than in controllers or Blazor components.
- `DoctorPilot.Infrastructure` implements persistence, repositories, authentication, and external services.
- `DoctorPilot.Api` should keep controllers thin. Controllers should delegate application behavior instead of containing business logic.
- `DoctorPilot.Contracts` contains request/response contracts shared across boundaries. Do not expose EF Core persistence concerns through contracts.
- `DoctorPilot.Web.Migration` must not access the database or EF Core directly. Access backend functionality through the appropriate application/API boundaries and client services.

Before introducing a new pattern or abstraction, inspect similar existing features and follow the established project conventions unless there is a clear reason not to.

### Administrative Provisioning Invariants

- `SuperAdmin` is global and must have `ClinicaId = null`.
- Tenant users, including `TenantAdmnin`, `Medico`, and `Admnistrativo`, must have a `ClinicaId`.
- The first `TenantAdmnin` is provisioned with the clinic when a `SuperAdmin` creates it.
- `TenantAdmnin` does not imply that a corresponding `Profissional` exists; these are separate concepts.
- Do not assume `ICurrentUserService.ClinicaId` has a value. Tenant operations that require a clinic must use the existing required-clinic mechanism (`GetRequiredClinicaId()`).
- Do not create public endpoints for creating `SuperAdmin` users.
- Do not change the bootstrap to update the password or credentials of an existing `SuperAdmin` automatically.
- Preserve clinic isolation in all tenant operations.

## Agent Workflow

Before making non-trivial changes:

1. Inspect the relevant implementation and nearby examples.
2. Identify the projects and files affected by the requested change.
3. Preserve existing architectural patterns unless the task explicitly requires changing them.
4. Make the smallest focused change that satisfies the requirement.
5. Build the affected solution or projects.
6. Run relevant tests when available.
7. Review the final diff for unrelated or accidental changes.

For architectural changes, authentication changes, database schema changes, or changes spanning multiple projects, explain the proposed approach before editing files when the user asks for analysis or planning first.

Do not refactor unrelated code while implementing a feature or bug fix.

Do not introduce new libraries, architectural patterns, or infrastructure when the existing project already provides an appropriate solution.

## Repository Safety

Do not perform destructive Git or database operations unless explicitly requested.

Do not run commands such as:

- `git reset --hard`
- `git clean -fd`
- `git push --force`
- branch deletion
- database drop/delete commands

Do not commit or push changes unless explicitly requested.

Before modifying an existing EF Core migration or generating a migration with potentially destructive schema changes, inspect and explain the expected database impact.

Never remove or overwrite unrelated uncommitted user changes.

## Project Documentation

Project documentation is stored under `documentos/`.

- `documentos/Especificação do Projeto.md` describes functional requirements and the intended product behavior. Some technical/modeling sections may be outdated and must not be treated as the source of truth for the current implementation.
- `documentos/architecture.md` describes the current solution architecture and project responsibilities.
- `documentos/authentication.md` describes the current authentication and authorization flow.
- `documentos/database.md` describes the current persistence model and important database decisions.

When documentation conflicts with the current implementation, inspect the code and determine which information is outdated before making changes.
