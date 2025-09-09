defmodule CoreEvents.EventResource do
  use Ash.Resource,
    domain: CoreEvents.Domain,
    data_layer: AshPostgres.DataLayer

  postgres do
    table "events"
    repo CoreEvents.Repo
  end

  actions do
    defaults [:read, :create, :update, :destroy]
  end

  attributes do
    integer_primary_key :id
    attribute :name, :string, allow_nil?: false
    attribute :starts_at, :utc_datetime, allow_nil?: false
    attribute :ends_at, :utc_datetime
    attribute :venue, :string
    create_timestamp :inserted_at
    update_timestamp :updated_at
  end
end
