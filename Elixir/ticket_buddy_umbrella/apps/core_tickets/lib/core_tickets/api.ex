defmodule CoreTickets.Domain do
  use Ash.Domain,
    extensions: [AshJsonApi.Domain]

  resources do
    resource(CoreTickets.TicketResource)
  end
end
