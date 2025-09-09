defmodule CoreEvents.Event do
  use Ecto.Schema
  import Ecto.Changeset

  schema "events" do
    field :name, :string
    field :starts_at, :utc_datetime
    field :ends_at, :utc_datetime
    field :venue, :string
    timestamps(type: :utc_datetime)
  end

  def changeset(event, attrs) do
    event
    |> cast(attrs, [:name, :starts_at, :ends_at, :venue])
    |> validate_required([:name, :starts_at])
  end
end
