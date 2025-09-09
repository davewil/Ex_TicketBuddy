
# Concise plan: migrate backend to Elixir/BEAM

## Goals

- Keep system running while migrating incrementally.
- Match existing service boundaries (Events, Users, Tickets).
- Maintain RabbitMQ messaging and observability.

## Target Architecture

- Phoenix umbrella with apps:
  - core_events, core_users, core_tickets (Ecto domains)
  - api_gateway (Phoenix HTTP) or one Phoenix app per bounded context
  - messaging (Broadway for RabbitMQ inbox; Oban outbox publisher)
  - shared_telemetry (OpenTelemetry setup)
- Datastore: Postgres with Ecto (migrate off SQL Server).
- Messaging: Broadway (inbox) + Ecto outbox + Oban workers (publisher).
- Telemetry: OpenTelemetry OTLP exporter to existing collector.

## Ash integration points

- Domain & data

  - Model Events, Users, Tickets as Ash resources; use AshPostgres for repo/migrations.
  - Keep boundaries by grouping resources under per-context Ash APIs.

- APIs

  - For fast CRUD parity, expose resources via AshJsonApi (or AshGraphql if desired).
  - Use Phoenix controllers when you must match existing non-JSON:API shapes.

- Auth

  - Centralize authorization with Ash.Policy per action/attribute (good fit for Users/Tickets).

- Messaging

  - Use resource changes/after_action hooks to write to the outbox and enqueue Oban jobs.

- Ops

  - Ash emits telemetry you can bridge to OpenTelemetry; AshAdmin can serve as a temporary admin UI.

## Phased Migration

- Foundations

  - Bootstrap Phoenix umbrella; add deps: phoenix, ecto_sql, postgrex, broadway_rabbitmq, oban, opentelemetry, opentelemetry_exporter.
  - Mirror bounded contexts (Events, Users, Tickets) and define public contexts APIs.
  - Add Ash deps (ash, ash_postgres, ash_json_api) and create minimal resources for each context.

- Data Strategy

  - Stand Postgres next to SQL Server.
  - New Elixir writes go to Postgres; backfill from SQL Server via ETL/CDC.
  - Cut reads to Postgres after validation; retire SQL Server.
  - Use ash_postgres migrations (generate/verify) to evolve schema deterministically.

- API Parity

  - Recreate .NET endpoints in Phoenix controllers.
  - Run .NET and Elixir APIs side-by-side on different ports via Microservices/docker-compose.
  - Gradually point UI to Elixir routes.
  - Prefer AshJsonApi for quick CRUD; use controllers only where response format must match existing contracts.

- Messaging

  - Consumers: Broadway reading from current RabbitMQ queues/bindings.
  - Producers: Ecto outbox + Oban workers publishing to RabbitMQ.
  - Keep JSON contracts stable; add contract tests.
  - Trigger outbox enqueue from Ash after_action hooks to keep side effects consistent.

- Telemetry/Observability

  - Enable OpenTelemetry in Elixir; export OTLP to your collector.
  - Propagate W3C trace context across HTTP and RabbitMQ headers.
  - Forward Ash telemetry to OTel; add spans around Ash actions where needed.

- CI/CD & Docker

  - Add BEAM build/test job to CI.
  - Extend docker-compose to include BEAM services on same network and RabbitMQ.
  - Include ash_postgres.generate_migrations and verify steps in CI to prevent drift.

## Cutover Steps

- Launch Elixir Events API; mirror GET traffic for shadow reads.
- Switch POST/PUT for Events to Elixir, then Users, then Tickets.
- Move message production/consumption to Elixir once endpoints stable.
- Decommission .NET services per bounded context.

## Risks & Mitigations

- SQL Server driver maturity in Ecto → migrate to Postgres early.
- Contract drift → shared JSON schemas + contract tests.
- Trace gaps → standardize W3C headers; verify in dashboards.

## Next

- I can scaffold the Phoenix umbrella and docker-compose entries as a starter, on request.
