::: {align="center"}
# DonationHub

### A Secure Donation & Campaign Management Backend

A modular RESTful API built with **ASP.NET Core 10**, **Clean
Architecture**, and **CQRS** to support charitable campaigns, donors,
and transparent donation workflows.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Backend-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![ASP.NET
Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![Entity Framework
Core](https://img.shields.io/badge/EF_Core-10-512BD4?logo=nuget&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL
Server](https://img.shields.io/badge/Database-SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-TBD-lightgrey.svg)](#license)
:::

------------------------------------------------------------------------

## Overview

**DonationHub** is a backend project for a charitable donation platform.
It is designed around a layered architecture that separates business
rules, application use cases, infrastructure concerns, and HTTP
delivery.

The project focuses on building a maintainable API with secure
authentication, role-based access control, category management,
validation, and a foundation for campaign and donation workflows.

> **Project status:** Development is ongoing. This README describes the
> current repository structure and implemented foundations; features
> still under development are identified separately.

## Core Capabilities

### Authentication & Identity

-   User registration and login using ASP.NET Core Identity.
-   JWT access tokens with a **15-minute** lifetime.
-   Refresh-token generation and SHA-256 hash storage.
-   Token refresh and rotation workflow.
-   Authenticated user profile endpoint (`AuthMe`).
-   Profile update and password change operations.
-   Role-based authorization for protected operations.

### Category Management

-   Public category reading.
-   Admin-only category creation, update, and deletion.
-   Category responses shaped as `CategoryDto`.
-   Arabic and English category names and descriptions.
-   Automatic seeding of the five initial categories:
    -   Health --- الصحة
    -   Education --- التعليم
    -   Relief --- الإغاثة
    -   Orphans --- الأيتام
    -   Environment --- البيئة
-   Protection against deleting a category that is referenced by
    campaigns.

### Application & Domain Foundations

-   Clean separation of Domain, Application, Infrastructure, and API
    projects.
-   CQRS-style request handling with MediatR.
-   Request validation with FluentValidation.
-   Entity Framework Core persistence with SQL Server.
-   Centralized dependency registration and database initialization.

## Architecture

The solution is organized into four projects:

``` text
DonationHub/
│
├── DonationHub.Api/
│   ├── Controllers/
│   ├── Extension/
│   └── Program.cs
│
├── DonationHub.Application/
│   ├── Features/
│   ├── Interfaces/
│   └── DependencyInjection.cs
│
├── DonationHub.Domain/
│   ├── Entities/
│   └── ValueObjects/
│
├── DonationHub.Infrastructure/
│   ├── Data/
│   ├── Identity/
│   └── DependencyInjection.cs
│
├── DonationHub.slnx
└── .gitignore
```

  -----------------------------------------------------------------------
  Project                             Responsibility
  ----------------------------------- -----------------------------------
  `DonationHub.Domain`                Core entities, value objects, and
                                      business rules.

  `DonationHub.Application`           Use cases, CQRS handlers,
                                      validators, and application
                                      interfaces.

  `DonationHub.Infrastructure`        EF Core, SQL Server, Identity,
                                      persistence, and infrastructure
                                      implementations.

  `DonationHub.Api`                   HTTP controllers, middleware,
                                      dependency composition, and API
                                      configuration.
  -----------------------------------------------------------------------

The intended dependency direction keeps the Domain independent from
infrastructure and delivery details.

## Technology Stack

  Technology                 Purpose
  -------------------------- -----------------------------
  C# / .NET 10               Main development platform
  ASP.NET Core Web API       HTTP API
  Entity Framework Core 10   ORM and data access
  SQL Server                 Relational database
  ASP.NET Core Identity      User and role management
  JWT Bearer                 API authentication
  MediatR 14                 Request/handler dispatching
  FluentValidation 12        Request validation
  Scalar                     Interactive API reference
  OpenAPI                    API specification

## Getting Started

### Prerequisites

Install the following before running the project:

-   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
-   [SQL Server](https://www.microsoft.com/sql-server) (or a compatible
    SQL Server instance)
-   [Git](https://git-scm.com/)
-   An IDE such as [Visual Studio](https://visualstudio.microsoft.com/)
    or [Visual Studio Code](https://code.visualstudio.com/)

Verify the SDK installation:

``` bash
dotnet --version
```

### 1. Clone the repository

``` bash
git clone https://github.com/ziadtaher8545/DonationHub.git
cd DonationHub
```

### 2. Configure application settings

Configure the database connection string and authentication settings
using **User Secrets** or environment variables. Do not commit
credentials, connection strings containing passwords, or JWT signing
keys.

From the API project directory, initialize User Secrets if they have not
been initialized:

``` bash
cd DonationHub.Api
dotnet user-secrets init
```

Set your SQL Server connection string (replace the example with your own
local configuration):

``` bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=DonationHubDb;Trusted_Connection=True;TrustServerCertificate=True"
```

Configure the JWT and initial administrator settings using the option
names expected by the application configuration classes. Keep the
signing key private and use a sufficiently strong random secret.

> Configuration section and property names must match the options
> classes in the checked-out version of the project. Never paste real
> secrets into source control or public issue reports.

### 3. Restore dependencies

From the solution root:

``` bash
dotnet restore
```

### 4. Apply database migrations

If migrations are already included in the repository, run:

``` bash
dotnet ef database update \
  --project DonationHub.Infrastructure \
  --startup-project DonationHub.Api
```

If the Entity Framework CLI is not installed:

``` bash
dotnet tool install --global dotnet-ef
```

If your local environment uses PowerShell, the migration command can be
entered on one line:

``` powershell
dotnet ef database update --project DonationHub.Infrastructure --startup-project DonationHub.Api
```

### 5. Run the API

From the solution root:

``` bash
dotnet run --project DonationHub.Api
```

The application initializes its database during startup. Ensure the
configured SQL Server instance is available and the database credentials
are valid.

## API Documentation

In the current application configuration, OpenAPI and Scalar are mapped
in the **Development** environment.

When the API is running, open the Scalar route exposed by the
application (commonly `/scalar`).

The OpenAPI document is mapped by `MapOpenApi()` (commonly
`/openapi/v1.json`).

> **Authentication documentation note:** The current checked-in
> `Program.cs` calls `AddOpenApi()` without a JWT Bearer security-scheme
> transformer. Add the Bearer scheme to the OpenAPI document if you want
> Scalar's Authorize workflow to describe JWT authentication correctly.
> API authentication middleware is configured separately from OpenAPI
> documentation.

## Authorization Model

The category-management access policy is:

  Operation         Access
  ----------------- --------
  Read categories   Public
  Create category   Admin
  Update category   Admin
  Delete category   Admin

Authenticated endpoints use JWT Bearer authentication. Role-based
authorization is applied to restricted operations.

## Development Notes

-   Keep business rules in the Domain layer where appropriate.
-   Keep application workflows in handlers rather than controllers.
-   Use validators for request input validation.
-   Keep infrastructure implementations behind Application abstractions.
-   Use database constraints as a final safeguard for data integrity.
-   Use UTC for persisted timestamps where required by the application
    contract.
-   Keep secrets in User Secrets or environment variables.
-   Do not commit generated `bin/` or `obj/` directories.

## Roadmap

The following areas represent the broader platform direction and should
be marked complete only when their implementation and tests are
finished:

-   [ ] Complete and verify refresh-token rotation, reuse detection, and
    token-family revocation.
-   [ ] Complete logout and session-revocation behavior.
-   [ ] Finish and verify campaign management workflows.
-   [ ] Finish and verify donation processing and campaign balance
    updates.
-   [ ] Expand automated unit and integration test coverage.
-   [ ] Complete OpenAPI JWT Bearer security documentation.
-   [ ] Add CI build and test validation.

## Contributing

Contributions and suggestions are welcome.

1.  Fork the repository.
2.  Create a feature branch.
3.  Keep changes focused and follow the existing project architecture.
4.  Run the build and relevant tests.
5.  Open a pull request with a clear description of the change.

## License

No license has been specified for this repository yet. Unless a license
is added, standard copyright restrictions apply and reuse, modification,
and distribution are not automatically granted.

------------------------------------------------------------------------

::: {align="center"}
**Built with ASP.NET Core 10 and Clean Architecture.**
:::
