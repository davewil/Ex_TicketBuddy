defmodule CoreTickets.TicketResource do
  use Ash.Resource,
    domain: CoreTickets.Domain,
    data_layer: AshPostgres.DataLayer

  postgres do
    table "tickets"
    repo CoreTickets.Repo
  end

  actions do
    defaults [:read, :create, :update, :destroy]
  end

  attributes do
    integer_primary_key :id
    attribute :event_id, :integer, allow_nil?: false
    attribute :user_id, :integer, allow_nil?: false
    attribute :price_cents, :integer, allow_nil?: false
    attribute :status, :atom, constraints: [one_of: [:reserved, :purchased, :refunded]], allow_nil?: false, default: :reserved
    create_timestamp :inserted_at
    update_timestamp :updated_at
  end
end
