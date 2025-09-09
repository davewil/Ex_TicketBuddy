defmodule CoreTickets.TicketResource do
  use Ash.Resource,
    domain: CoreTickets.Domain,
    data_layer: AshPostgres.DataLayer,
    extensions: [AshJsonApi.Resource],
    authorizers: [Ash.Policy.Authorizer]

  postgres do
    table "tickets"
    repo CoreTickets.Repo
  end

  json_api do
    type "tickets"
    routes do
      base "/tickets"
      get :read
      index :read
      post :create
      patch :update
      delete :destroy
    end
  end

  policies do
    # Basic authorization - in a real app you'd check user ownership
    policy always() do
      authorize_if always()
    end
  end

  actions do
    defaults [:read, :destroy]

    create :create do
      primary? true
      accept [:event_id, :user_id, :price_cents, :status]
    end

    update :update do
      primary? true
      accept [:price_cents, :status]
    end
  end

  attributes do
    integer_primary_key :id
    attribute :event_id, :integer, allow_nil?: false, public?: true
    attribute :user_id, :integer, allow_nil?: false, public?: true
    attribute :price_cents, :integer, allow_nil?: false, public?: true
    attribute :status, :atom,
      constraints: [one_of: [:reserved, :confirmed, :cancelled, :purchased, :refunded]],
      allow_nil?: false,
      default: :reserved,
      public?: true
    create_timestamp :inserted_at
    update_timestamp :updated_at
  end
end
