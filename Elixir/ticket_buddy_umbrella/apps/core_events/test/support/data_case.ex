defmodule CoreEvents.DataCase do
  @moduledoc """
  This module defines the setup for tests requiring
  access to the CoreEvents data layer.
  """

  use ExUnit.CaseTemplate

  using do
    quote do
      alias CoreEvents.Repo

      import Ecto
      import Ecto.Changeset
      import Ecto.Query
      import CoreEvents.DataCase
    end
  end

  setup tags do
    :ok = Ecto.Adapters.SQL.Sandbox.checkout(CoreEvents.Repo)

    unless tags[:async] do
      Ecto.Adapters.SQL.Sandbox.mode(CoreEvents.Repo, {:shared, self()})
    end

    :ok
  end
end
