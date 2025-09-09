# Migration to Elixir/BEAM – Execution Checklist

Use this checklist to track the incremental migration from .NET to Elixir/BEAM. Check items when completed and link the related GitHub issue next to each task.

Legend: [ ] = Todo, [~] = In progress, [x] = Done

Owner: ______  |  Target start: ______  |  Target finish: ______

## 1) Foundations (Phoenix + Ash + Core deps)

- [ ] Phoenix umbrella scaffold created with apps:
  - [ ] core_events
  - [ ] core_users
  - [ ] core_tickets
  - [ ] messaging (Broadway/Oban integration)
  - [ ] shared_telemetry (OTel setup)
  - [ ] api_gateway (Phoenix HTTP) or per-context Phoenix apps
- [ ] Dependencies added and compiled:
  - [ ] phoenix, phoenix_ecto, ecto_sql, postgrex
  - [ ] broadway, broadway_rabbitmq, oban
  - [ ] opentelemetry, opentelemetry_exporter
  - [ ] ash, ash_postgres, ash_json_api
- [ ] Repo configuration and Postgres connection (dev/test)
- [ ] Oban configured (queues, pruning) and migrations generated
- [ ] Broadway base project scaffolding (RabbitMQ connectivity tested)
- [ ] OpenTelemetry configured with OTLP exporter and basic spans visible in collector

## 2) Dev environment & Docker

- [ ] Add Postgres service in docker-compose alongside existing SQL Server
- [ ] Ensure RabbitMQ is reachable by BEAM apps (same network, credentials)
- [ ] Compose BEAM services with .NET in Microservices/docker-compose (side-by-side ports)
- [ ] Local run scripts/tasks to start .NET + Elixir stacks together
- [ ] Connection strings and env vars for both stacks documented

## 3) Minimal Ash resources (per bounded context)

- [ ] Events: resources modeled and grouped under an Ash API
- [ ] Users: resources modeled and grouped under an Ash API
- [ ] Tickets: resources modeled and grouped under an Ash API
- [ ] AshPostgres migrations generated and applied (dev/test)
- [ ] Seed scripts and sample data for smoke tests
- [ ] Ash.Policy authorization baseline for Users/Tickets

## 4) Data migration (SQL Server -> Postgres)

- [ ] Decide ETL/CDC approach (e.g., Debezium, periodic ETL, custom sync)
- [ ] Initial backfill job written and tested
- [ ] Data parity validation (row counts, key invariants, spot checks)
- [ ] New Elixir writes go to Postgres (feature-flagged)
- [ ] Gradual read cutover to Postgres after validation
- [ ] Decommission SQL Server after full parity period

## 5) API parity

- [ ] Inventory .NET endpoints to replicate, per context (Events, Users, Tickets)
- [ ] Implement initial Phoenix controllers or AshJsonApi for CRUD parity
- [ ] Shadow GET traffic to Elixir (compare responses/log diffs)
- [ ] Shared JSON schemas and contract tests added
- [ ] UI gradually pointed to Elixir routes for selected endpoints
- [ ] Expand coverage until full parity is achieved

## 6) Messaging

- [ ] Broadway consumers: connect to existing RabbitMQ queues/bindings
- [ ] Ecto Outbox table created; publisher worker implemented with Oban
- [ ] Ash after_action hooks enqueue domain events to outbox
- [ ] Publisher emits messages to RabbitMQ with stable JSON contracts
- [ ] Contract tests for message formats; header propagation checked

## 7) Telemetry / Observability

- [ ] OpenTelemetry exporter configured to existing collector
- [ ] W3C trace context propagated across HTTP and RabbitMQ headers
- [ ] Ash telemetry bridged to OTel; spans around key Ash actions
- [ ] Dashboards and alerts updated to include BEAM services

## 8) CI/CD & Quality gates

- [ ] Add BEAM build/test workflow to CI
- [ ] Include ash_postgres.generate_migrations and verification steps to prevent drift
- [ ] Build container images for umbrella apps where applicable
- [ ] Contract tests and smoke tests in pipeline
- [ ] Documentation and changelog updated per iteration

## 9) Incremental cutover

- [ ] Launch Elixir Events API; mirror GET traffic
- [ ] Switch POST/PUT for Events to Elixir
- [ ] Switch Users to Elixir
- [ ] Switch Tickets to Elixir
- [ ] Move message production/consumption entirely to Elixir
- [ ] Decommission corresponding .NET services per bounded context

## 10) Risks & mitigations

- [ ] SQL Server driver maturity → migrate to Postgres early; confirm decision
- [ ] Contract drift → shared schemas + contract tests enforced
- [ ] Trace gaps → standardize headers; validate in dashboards

## 11) Governance / tracking

- [ ] Create GitHub labels for migration categories
- [ ] Create milestones per migration phase
- [ ] Create issues for each task (see plans/github_issues.ps1)
- [ ] Weekly progress review and status update

Notes:
- Keep the .NET and Elixir stacks running side-by-side until cutover completes.
- Prefer AshJsonApi for fast CRUD; use Phoenix controllers to match bespoke responses.
- Keep contracts stable across HTTP and messaging; use contract tests for safety.

