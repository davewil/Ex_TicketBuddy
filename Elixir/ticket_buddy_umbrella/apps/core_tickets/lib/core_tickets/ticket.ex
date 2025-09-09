defmodule CoreTickets.Ticket do
  use Ecto.Schema
  import Ecto.Changeset

  schema "tickets" do
    field :event_id, :id
    field :user_id, :id
    field :price_cents, :integer
    field :status, Ecto.Enum, values: [:reserved, :purchased, :refunded]
    timestamps(type: :utc_datetime)
  end

  def changeset(ticket, attrs) do
    ticket
    |> cast(attrs, [:event_id, :user_id, :price_cents, :status])
    |> validate_required([:event_id, :user_id, :price_cents])
    |> check_constraint(:price_cents, name: :price_cents_non_negative)
  end
end
