defmodule CoreUsers.Application do
  @moduledoc false
  use Application

  @impl true
  def start(_type, _args) do
    children = [
      CoreUsers.Repo
    ]

    opts = [strategy: :one_for_one, name: CoreUsers.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
