# Ticket Buddy – Elixir Umbrella

This folder will contain the Phoenix umbrella that mirrors the repo’s bounded contexts.

## Scaffold

Run from repo root:

```powershell
pwsh -File .\scripts\scaffold_elixir_umbrella.ps1
```

This creates:

- `Elixir/ticket_buddy_umbrella/`
  - `apps/api_gateway/` (Phoenix API, no Ecto/assets)
  - `apps/core_events/`, `apps/core_users/`, `apps/core_tickets/` (domain libs)
  - `apps/messaging/` (Broadway/Oban)
  - `apps/shared_telemetry/` (OpenTelemetry)

## Next steps

1) Add dependencies in each `mix.exs`:

- api_gateway: phoenix, plug_cowboy, umbrella deps on core_* / messaging / shared_telemetry
- core_*: ecto_sql, postgrex, shared_telemetry (later ash, ash_postgres)
- messaging: broadway, broadway_rabbitmq, oban, ecto_sql, postgrex
- shared_telemetry: opentelemetry, opentelemetry_exporter

1) Create per-context Repos (`CoreX.Repo`) and configure `:ecto_repos` + `DATABASE_URL` in `config/dev.exs`.

1) Bootstrap OpenTelemetry in `shared_telemetry` (exporter to existing collector).

1) Fetch/compile and run API gateway:

```powershell
cd .\Elixir\ticket_buddy_umbrella
mix deps.get
mix compile
cd .\apps\api_gateway
mix phx.server
```

Health check: <http://localhost:4000/health>
