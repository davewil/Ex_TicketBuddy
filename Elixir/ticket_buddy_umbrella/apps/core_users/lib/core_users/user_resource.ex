defmodule CoreUsers.UserResource do
  use Ash.Resource,
    domain: CoreUsers.Domain,
    data_layer: AshPostgres.DataLayer,
    extensions: [AshJsonApi.Resource],
    authorizers: [Ash.Policy.Authorizer]

  postgres do
    table("users")
    repo(CoreUsers.Repo)
  end

  json_api do
    type("users")

    routes do
      base("/users")
      get(:read)
      index(:read)
      post(:create)
      patch(:update)
      delete(:destroy)
    end
  end

  policies do
    # Allow all for now - in a real app you'd have proper authentication
    policy always() do
      authorize_if(always())
    end
  end

  actions do
    defaults([:read, :destroy])

    create :create do
      primary?(true)
      accept([:name, :email])
      validate(present(:name))
      validate(match(:email, "@"))
    end

    update :update do
      primary?(true)
      accept([:name, :email])
    end
  end

  attributes do
    integer_primary_key(:id)
    attribute(:email, :string, allow_nil?: false, public?: true)
    attribute(:name, :string, public?: true)
    create_timestamp(:inserted_at)
    update_timestamp(:updated_at)
  end
end
