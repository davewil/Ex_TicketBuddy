# Ticket Buddy

A practical reference repo that:

- Compares a Modular Monolith and a Microservices architecture for the same domain (Events, Users, Tickets) in .NET.
- Tracks an incremental migration of the backend to Elixir/BEAM (Phoenix + Ash), keeping messaging and observability intact.

## Migration status

Overall migration progress:

![Migration Progress](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-progress.svg)

Per-phase progress:

![Foundations](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-foundations.svg)
![Dev Env](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-dev-env.svg)
![Ash Resources](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-ash-resources.svg)
![Data Migration](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-data-migration.svg)
![API Parity](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-api-parity.svg)
![Messaging](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-messaging.svg)
![Observability](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-observability.svg)
![CI/CD](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-ci-cd.svg)
![Cutover](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-cutover.svg)
![Governance](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-governance.svg)

Details:

- Concise migration plan: `plans/concise_elixir_migration.md`
- Execution checklist with links to issues: `plans/migrations_checklist.md`
- Issues labeled `migration` track progress (see GitHub Issues tab)

## Architectures in this repo

### Modular Monolith (.NET)

- Well-defined, in-repo modules hosted as a single application
- Asynchronous messaging between modules via MassTransit + RabbitMQ
- Recommended starting point for new products

Docs: [Modular Monolith](./ModularMonolith/README.md)

### Microservices (.NET)

- Distributed services communicating via RabbitMQ (MassTransit)
- Fully observable with OpenTelemetry and .NET Aspire
- Demonstrates patterns like YARP gateway and transactional outbox

Docs: [Microservices](./Microservices/README.md)

### Elixir/BEAM Target (In Progress)

- Phoenix umbrella with Ash framework and AshPostgres
- Broadway for RabbitMQ messaging, Oban for background jobs
- OpenTelemetry integration for observability continuity

Docs: [Elixir Setup](./Elixir/README.md)

### Target Elixir/BEAM backend

- Phoenix umbrella with bounded contexts (Events, Users, Tickets) ✅
- Ash resources + AshPostgres, Broadway (RabbitMQ), Oban, OpenTelemetry 🔄
- Gradual cutover while keeping APIs and message contracts stable

**Current Status**: Foundation complete with Ash framework integrated. Core Ash resources created for all three bounded contexts (Events, Users, Tickets) with proper domain modeling. Phoenix server runs successfully with health endpoint operational.

**Recent Progress**:

- ✅ Phoenix umbrella scaffolding with 6 apps
- ✅ All core dependencies (Phoenix, Ash, Broadway, Oban, OpenTelemetry)
- ✅ Ecto repositories and Postgres connectivity
- ✅ Ash.Domain pattern implementation for modern Ash architecture
- ✅ Working health API endpoint
- 🔄 Ash resources (Events, Users, Tickets domains created)

Plan: see `plans/concise_elixir_migration.md`

## Repository structure

- `ModularMonolith/` – .NET modular monolith solution
- `Microservices/` – .NET microservices solution
- `Elixir/` – Phoenix umbrella with Ash framework (migration target)
- `UI/` – React + Vite client
- `plans/` – Migration plan, checklist, and automation scripts
- `.github/workflows/` – CI and migration badge automation

## Key technologies

- .NET: ASP.NET Core, MassTransit, RabbitMQ, OpenTelemetry, .NET Aspire, YARP, Redis
- Frontend: React + Vite + Vitest
- Elixir (planned): Phoenix, Ash, AshPostgres, Broadway RabbitMQ, Oban, OpenTelemetry

## Observability

OpenTelemetry is used across services. The .NET stacks surface telemetry in the Aspire dashboard; the Elixir stack will export OTLP to the same collector.

![Observability Architecture](./Observability.png)
