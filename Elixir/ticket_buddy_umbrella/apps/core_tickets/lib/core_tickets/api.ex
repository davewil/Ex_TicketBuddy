defmodule CoreTickets.Domain do
  use Ash.Domain,
    extensions: [AshJsonApi.Domain]

  resources do
    resource CoreTickets.TicketResource do
      # Code interface definitions (per-resource form for current Ash version)
      define :create_ticket, action: :create
      define :get_ticket, action: :read, get_by: [:id]
      define :update_ticket, action: :update
      define :process_reservation, action: :process_reservation
  define :list_confirmed_tickets, action: :list_confirmed
    end
  end
end
