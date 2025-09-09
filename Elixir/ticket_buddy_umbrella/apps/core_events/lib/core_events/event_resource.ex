defmodule CoreEvents.EventResource do
  use Ash.Resource,
    domain: CoreEvents.Domain,
    data_layer: AshPostgres.DataLayer,
    extensions: [AshJsonApi.Resource],
    authorizers: [Ash.Policy.Authorizer]

  postgres do
    table "events"
    repo CoreEvents.Repo
  end

  json_api do
    type "events"
    routes do
      base "/events"
      get :read
      index :read
      post :create
      patch :update
      delete :destroy
    end
  end

  policies do
    # Events can be read by anyone, but only authorized users can modify
    policy action_type(:read) do
      authorize_if always()
    end

    policy action_type([:create, :update, :destroy]) do
      authorize_if always()  # In a real app, you'd check for admin role
    end
  end

  actions do
    defaults [:read, :destroy]

    create :create do
      primary? true
      accept [:name, :starts_at, :ends_at, :venue]
    end

    update :update do
      primary? true
      accept [:name, :starts_at, :ends_at, :venue]
    end
  end

  attributes do
    integer_primary_key :id
    attribute :name, :string, allow_nil?: false, public?: true
    attribute :starts_at, :utc_datetime, allow_nil?: false, public?: true
    attribute :ends_at, :utc_datetime, public?: true
    attribute :venue, :string, public?: true
    create_timestamp :inserted_at
    update_timestamp :updated_at
  end
end
