defmodule CoreTickets.DataCase do
  @moduledoc """
  This module defines the setup for tests requiring
  access to the CoreTickets data layer.
  """

  use ExUnit.CaseTemplate

  using do
    quote do
      alias CoreTickets.Repo

      import Ecto
      import Ecto.Changeset
      import Ecto.Query
      import CoreTickets.DataCase
    end
  end

  setup tags do
    # Checkout sandbox connections for all repos used in these tests
    :ok = Ecto.Adapters.SQL.Sandbox.checkout(CoreTickets.Repo)
    :ok = Ecto.Adapters.SQL.Sandbox.checkout(CoreUsers.Repo)
    :ok = Ecto.Adapters.SQL.Sandbox.checkout(CoreEvents.Repo)

    unless tags[:async] do
      Ecto.Adapters.SQL.Sandbox.mode(CoreTickets.Repo, {:shared, self()})
      Ecto.Adapters.SQL.Sandbox.mode(CoreUsers.Repo, {:shared, self()})
      Ecto.Adapters.SQL.Sandbox.mode(CoreEvents.Repo, {:shared, self()})
    end

    :ok
  end
end
