# Migration to Elixir/BEAM – Execution Checklist

![Migration Progress](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-progress.svg)

Use this checklist to track the incremental migration from .NET to Elixir/BEAM. Check items when completed and link the related GitHub issue next to each task.

Legend: [ ] = Todo, [~] = In progress, [x] = Done

Owner: ______  |  Target start: ______  |  Target finish: ______

## 1) Foundations (Phoenix + Ash + Core deps)

![Foundations](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-foundations.svg)

- [ ] Phoenix umbrella scaffold created with apps: ([#3](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/3))
  - [ ] core_events
  - [ ] core_users
  - [ ] core_tickets
  - [ ] messaging (Broadway/Oban integration)
  - [ ] shared_telemetry (OTel setup)
  - [ ] api_gateway (Phoenix HTTP) or per-context Phoenix apps
- [ ] Dependencies added and compiled: ([#4](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/4))
  - [ ] phoenix, phoenix_ecto, ecto_sql, postgrex
  - [ ] broadway, broadway_rabbitmq, oban
  - [ ] opentelemetry, opentelemetry_exporter
  - [ ] ash, ash_postgres, ash_json_api
- [ ] Repo configuration and Postgres connection (dev/test) ([#5](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/5))
- [ ] Oban configured (queues, pruning) and migrations generated ([#6](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/6))
- [ ] Broadway base project scaffolding (RabbitMQ connectivity tested) ([#7](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/7))
- [ ] OpenTelemetry configured with OTLP exporter and basic spans visible in collector ([#8](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/8))

## 2) Dev environment & Docker

![Dev Env](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-dev-env.svg)

- [ ] Add Postgres service in docker-compose alongside existing SQL Server ([#9](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/9))
- [ ] Ensure RabbitMQ is reachable by BEAM apps (same network, credentials) ([#10](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/10))
- [ ] Compose BEAM services with .NET in Microservices/docker-compose (side-by-side ports) ([#11](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/11))
- [ ] Local run scripts/tasks to start .NET + Elixir stacks together ([#12](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/12))
- [ ] Connection strings and env vars for both stacks documented ([#13](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/13))

## 3) Minimal Ash resources (per bounded context)

![Ash Resources](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-ash-resources.svg)

- [ ] Events: resources modeled and grouped under an Ash API ([#14](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/14))
- [ ] Users: resources modeled and grouped under an Ash API ([#15](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/15))
- [ ] Tickets: resources modeled and grouped under an Ash API ([#16](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/16))
- [ ] AshPostgres migrations generated and applied (dev/test) ([#17](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/17))
- [ ] Seed scripts and sample data for smoke tests ([#18](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/18))
- [ ] Ash.Policy authorization baseline for Users/Tickets ([#19](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/19))

## 4) Data migration (SQL Server -> Postgres)

![Data Migration](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-data-migration.svg)

- [ ] Decide ETL/CDC approach (e.g., Debezium, periodic ETL, custom sync) ([#20](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/20))
- [ ] Initial backfill job written and tested ([#21](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/21))
- [ ] Data parity validation (row counts, key invariants, spot checks) ([#22](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/22))
- [ ] New Elixir writes go to Postgres (feature-flagged) ([#23](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/23))
- [ ] Gradual read cutover to Postgres after validation ([#24](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/24))
- [ ] Decommission SQL Server after full parity period ([#25](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/25))

## 5) API parity

![API Parity](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-api-parity.svg)

- [ ] Inventory .NET endpoints to replicate, per context (Events, Users, Tickets) ([#26](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/26))
- [ ] Implement initial Phoenix controllers or AshJsonApi for CRUD parity ([#27](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/27))
- [ ] Shadow GET traffic to Elixir (compare responses/log diffs) ([#28](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/28))
- [ ] Shared JSON schemas and contract tests added ([#29](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/29))
- [ ] UI gradually pointed to Elixir routes for selected endpoints ([#30](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/30))
- [ ] Expand coverage until full parity is achieved ([#31](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/31))

## 6) Messaging

![Messaging](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-messaging.svg)

- [ ] Broadway consumers: connect to existing RabbitMQ queues/bindings ([#32](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/32))
- [ ] Ecto Outbox table created; publisher worker implemented with Oban ([#33](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/33))
- [ ] Ash after_action hooks enqueue domain events to outbox ([#34](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/34))
- [ ] Publisher emits messages to RabbitMQ with stable JSON contracts ([#35](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/35))
- [ ] Contract tests for message formats; header propagation checked ([#36](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/36))

## 7) Telemetry / Observability

![Observability](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-observability.svg)

- [ ] OpenTelemetry exporter configured to existing collector ([#37](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/37))
- [ ] W3C trace context propagated across HTTP and RabbitMQ headers ([#38](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/38))
- [ ] Ash telemetry bridged to OTel; spans around key Ash actions ([#39](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/39))
- [ ] Dashboards and alerts updated to include BEAM services ([#40](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/40))

## 8) CI/CD & Quality gates

![CI/CD](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-ci-cd.svg)

- [ ] Add BEAM build/test workflow to CI ([#41](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/41))
- [ ] Include ash_postgres.generate_migrations and verification steps to prevent drift ([#42](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/42))
- [ ] Build container images for umbrella apps where applicable ([#43](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/43))
- [ ] Contract tests and smoke tests in pipeline ([#44](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/44))
- [ ] Documentation and changelog updated per iteration ([#45](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/45))

## 9) Incremental cutover

![Cutover](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-cutover.svg)

- [ ] Launch Elixir Events API; mirror GET traffic ([#46](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/46))
- [ ] Switch POST/PUT for Events to Elixir ([#47](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/47))
- [ ] Switch Users to Elixir ([#48](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/48))
- [ ] Switch Tickets to Elixir ([#49](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/49))
- [ ] Move message production/consumption entirely to Elixir ([#50](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/50))
- [ ] Decommission corresponding .NET services per bounded context ([#51](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/51))

## 10) Risks & mitigations

![Risks](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-progress.svg)

- [ ] SQL Server driver maturity → migrate to Postgres early; confirm decision ([#52](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/52))
- [ ] Contract drift → shared schemas + contract tests enforced ([#53](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/53))
- [ ] Trace gaps → standardize headers; validate in dashboards ([#54](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/54))

## 11) Governance / tracking

![Governance](https://raw.githubusercontent.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/main/.github/badges/migration-governance.svg)

- [ ] Create GitHub labels for migration categories ([#55](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/55))
- [ ] Create milestones per migration phase ([#1](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/1))
- [ ] Create issues for each task (see plans/github_issues.ps1)
- [ ] Weekly progress review and status update ([#2](https://github.com/davewil/Ex_TicketBuddy_ModularMonolith_To_Microservices/issues/2))

Notes:

- Keep the .NET and Elixir stacks running side-by-side until cutover completes.
- Prefer AshJsonApi for fast CRUD; use Phoenix controllers to match bespoke responses.
- Keep contracts stable across HTTP and messaging; use contract tests for safety.

