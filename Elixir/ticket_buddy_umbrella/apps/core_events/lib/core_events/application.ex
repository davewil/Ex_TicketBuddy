defmodule CoreEvents.Application do
  @moduledoc false
  use Application

  @impl true
  def start(_type, _args) do
    children = [
      CoreEvents.Repo
    ]

    opts = [strategy: :one_for_one, name: CoreEvents.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
