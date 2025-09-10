defmodule CoreTickets.Application do
  @moduledoc false
  use Application

  @impl true
  def start(_type, _args) do
    children = [
      CoreTickets.Repo
    ]

    opts = [strategy: :one_for_one, name: CoreTickets.Supervisor]
  {:ok, pid} = Supervisor.start_link(children, opts)
  CoreTickets.Telemetry.attach()
  {:ok, pid}
  end
end
