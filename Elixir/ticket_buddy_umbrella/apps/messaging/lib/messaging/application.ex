defmodule Messaging.Application do
  @moduledoc false
  use Application

  def start(_type, _args) do
    children = [
      # Oban supervisor (configured via config files)
      {Oban, Application.get_env(:messaging, Oban, [])},
      # Broadway consumer (starts only if enabled by config)
      Messaging.Consumers.RabbitConsumer
    ]

    opts = [strategy: :one_for_one, name: Messaging.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
