defmodule CoreTickets.TicketResource do
  use Ash.Resource,
    domain: CoreTickets.Domain,
    data_layer: AshPostgres.DataLayer,
    extensions: [AshJsonApi.Resource, AshOban],
    authorizers: [Ash.Policy.Authorizer]

  postgres do
    table("tickets")
    repo(CoreTickets.Repo)
  end

  json_api do
    type("tickets")

    routes do
      base("/tickets")
      get(:read)
      index(:read)
      post(:create)
      patch(:update)
      delete(:destroy)
    end
  end

  policies do
    # Allow AshOban to perform scheduled/triggered actions
    bypass AshOban.Checks.AshObanInteraction do
      authorize_if(always())
    end

    # Basic authorization - in a real app you'd check user ownership
    policy always() do
      authorize_if(always())
    end
  end

  actions do
    defaults([:read, :destroy])

    create :create do
      primary?(true)
      accept([:event_id, :user_id, :price_cents, :status])
    end

    update :update do
      primary?(true)
      accept([:price_cents, :status])
    end

    # Used by ash_oban on_error to mark a ticket as failed/cancelled
    update :errored do
      accept([])
      change set_attribute(:status, :cancelled)
    end
  end

  attributes do
    integer_primary_key(:id)
    attribute(:event_id, :integer, allow_nil?: false, public?: true)
    attribute(:user_id, :integer, allow_nil?: false, public?: true)
    attribute(:price_cents, :integer, allow_nil?: false, public?: true)

    attribute(:status, :atom,
      constraints: [one_of: [:reserved, :confirmed, :cancelled, :purchased, :refunded]],
      allow_nil?: false,
      default: :reserved,
      public?: true
    )

    create_timestamp(:inserted_at)
    update_timestamp(:updated_at)
  end

  # AshOban integration: minimal trigger scaffold (disabled by default)
  # When ready, set `scheduler_cron` to a cron expression (e.g., "* * * * *")
  # and add the corresponding queue (:ticket_resource_process) to Oban queues.
  oban do
    triggers do
      trigger :process do
        # No-op placeholder: re-run the primary update to demonstrate wiring
        action(:update)
  on_error(:errored)
        # Only consider tickets that are still reserved
        where(expr(status == :reserved))
  # Enabled to run every minute
  scheduler_cron("* * * * *")
        # Provide stable module names to avoid dangling jobs upon renames
        worker_module_name(CoreTickets.Oban.Workers.TicketResourceProcess)
        scheduler_module_name(CoreTickets.Oban.Schedulers.TicketResourceProcess)
      end
    end
  end
end
