defmodule CoreUsers.UserResource do
  use Ash.Resource,
    domain: CoreUsers.Domain,
    data_layer: AshPostgres.DataLayer

  postgres do
    table "users"
    repo CoreUsers.Repo
  end

  actions do
    defaults [:read, :create, :update, :destroy]
  end

  attributes do
    integer_primary_key :id
    attribute :email, :string, allow_nil?: false
    attribute :name, :string
    create_timestamp :inserted_at
    update_timestamp :updated_at
  end
end
