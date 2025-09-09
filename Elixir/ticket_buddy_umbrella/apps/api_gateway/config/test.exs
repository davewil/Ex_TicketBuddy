import Config

# Enable the SQL sandbox plug in tests so request processes share DB connections
config :api_gateway, :sql_sandbox, true
