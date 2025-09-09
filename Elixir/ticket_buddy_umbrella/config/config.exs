# This file is responsible for configuring your umbrella
# and **all applications** and their dependencies with the
# help of the Config module.
#
# Note that all applications in your umbrella share the
# same configuration and dependencies, which is why they
# all use the same configuration file. If you want different
# configurations or dependencies per app, it is best to
# move said applications out of the umbrella.
# This file is responsible for configuring your application
# and its dependencies with the aid of the Config module.
#
# This configuration file is loaded before any dependency and
# is restricted to this project.

# General application configuration
import Config

config :api_gateway,
  generators: [timestamp_type: :utc_datetime]

# Ecto repos for core apps
config :core_events, ecto_repos: [CoreEvents.Repo]
config :core_users, ecto_repos: [CoreUsers.Repo]
config :core_tickets, ecto_repos: [CoreTickets.Repo]

# Ash/AshPostgres configuration for CoreEvents
config :core_events, ash_domains: [CoreEvents.Domain]

# Ash/AshPostgres configuration for CoreUsers
config :core_users, ash_domains: [CoreUsers.Domain]

# Ash/AshPostgres configuration for CoreTickets
config :core_tickets, ash_domains: [CoreTickets.Domain]

# Configures the endpoint
config :api_gateway, ApiGatewayWeb.Endpoint,
  url: [host: "localhost"],
  adapter: Bandit.PhoenixAdapter,
  render_errors: [
    formats: [json: ApiGatewayWeb.ErrorJSON],
    layout: false
  ],
  pubsub_server: ApiGateway.PubSub,
  live_view: [signing_salt: "32PAJktD"]

# Configures the mailer
#
# By default it uses the "Local" adapter which stores the emails
# locally. You can see the emails in your browser, at "/dev/mailbox".
#
# For production it's recommended to configure a different adapter
# at the `config/runtime.exs`.
config :api_gateway, ApiGateway.Mailer, adapter: Swoosh.Adapters.Local

# Configures Elixir's Logger
config :logger, :console,
  format: "$time $metadata[$level] $message\n",
  metadata: [:request_id]

# Use Jason for JSON parsing in Phoenix
config :phoenix, :json_library, Jason

# Register JSON:API mime type for tests and runtime
config :mime, :types, %{
  "application/vnd.api+json" => ["json-api"],
  "application/json" => ["json"]
}

# Oban base config (overridden per env)
config :messaging, Oban,
  queues: [default: 10],
  plugins: [
    {Oban.Plugins.Pruner, max_age: 60 * 60 * 24}
  ]

# AshOban global config (optional)
# Configure how actors are persisted with jobs or disable auth in triggers if needed.
# config :ash_oban, actor_persister: MyApp.AshObanActorPersister
# config :ash_oban, authorize?: true

# Note for AshOban trigger queues:
# When enabling the TicketResource :process trigger, consider adding the queue below.
# Example: queues: [default: 10, ticket_resource_process: 5]

# Import environment specific config. This must remain at the bottom
# of this file so it overrides the configuration defined above.
import_config "#{config_env()}.exs"

# Sample configuration:
#
#     config :logger, :console,
#       level: :info,
#       format: "$date $time [$level] $metadata$message\n",
#       metadata: [:user_id]
#
