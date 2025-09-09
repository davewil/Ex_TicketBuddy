import Config

# We don't run a server during test. If one is required,
# you can enable the server option below.
config :api_gateway, ApiGatewayWeb.Endpoint,
  http: [ip: {127, 0, 0, 1}, port: 4002],
  secret_key_base: "/KMv0JU55Ta4weuLAKlzN/uktIyt1C7pJ1aTDSw8rK7YKuGyMyVQ/8qgx5RiKQ0H",
  server: false

# In test we don't send emails
config :api_gateway, ApiGateway.Mailer, adapter: Swoosh.Adapters.Test

# Disable swoosh api client as it is only required for production adapters
config :swoosh, :api_client, false

# Print only warnings and errors during test
config :logger, level: :warning

# Initialize plugs at runtime for faster test compilation
config :phoenix, :plug_init_mode, :runtime

# Test database configs (sql_sandbox pool)
config :core_events, CoreEvents.Repo,
  url: System.get_env("CORE_EVENTS_DATABASE_URL") || "ecto://postgres:postgres@localhost:5432/core_events_test",
  pool: Ecto.Adapters.SQL.Sandbox

config :core_users, CoreUsers.Repo,
  url: System.get_env("CORE_USERS_DATABASE_URL") || "ecto://postgres:postgres@localhost:5432/core_users_test",
  pool: Ecto.Adapters.SQL.Sandbox

config :core_tickets, CoreTickets.Repo,
  url: System.get_env("CORE_TICKETS_DATABASE_URL") || "ecto://postgres:postgres@localhost:5432/core_tickets_test",
  pool: Ecto.Adapters.SQL.Sandbox
