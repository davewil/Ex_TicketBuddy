defmodule CoreUsers.DataCase do
  @moduledoc """
  This module defines the setup for tests requiring
  access to the CoreUsers data layer.
  """

  use ExUnit.CaseTemplate

  using do
    quote do
      alias CoreUsers.Repo

      import Ecto
      import Ecto.Changeset
      import Ecto.Query
      import CoreUsers.DataCase
    end
  end

  setup tags do
    :ok = Ecto.Adapters.SQL.Sandbox.checkout(CoreUsers.Repo)

    unless tags[:async] do
      Ecto.Adapters.SQL.Sandbox.mode(CoreUsers.Repo, {:shared, self()})
    end

    :ok
  end
end
