defmodule CoreTickets.Domain do
  use Ash.Domain

  resources do
    resource CoreTickets.TicketResource
  end
end
