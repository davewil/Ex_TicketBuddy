defmodule CoreTickets.OutboxMessageResource do
  use Ash.Resource,
    domain: CoreTickets.Domain,
    data_layer: AshPostgres.DataLayer,
    authorizers: [Ash.Policy.Authorizer]

  postgres do
    table "ticket_outbox_messages"
    repo CoreTickets.Repo
  end

  policies do
    policy always() do
      authorize_if always()
    end
  end

  actions do
    defaults [:read]

    create :enqueue do
      accept [:topic, :event_type, :payload]
    end

    update :mark_published do
      accept []
      change set_attribute(:published_at, expr(now()))
    end
  end

  attributes do
    integer_primary_key :id

    attribute :topic, :string do
      allow_nil? false
      constraints min_length: 1
      public? true
    end

    attribute :event_type, :string do
      allow_nil? false
      constraints min_length: 1
      public? true
    end

    attribute :payload, :map do
      allow_nil? false
      public? true
    end

    attribute :published_at, :utc_datetime_usec do
      public? true
      allow_nil? true
    end

    create_timestamp :inserted_at
  end
end
