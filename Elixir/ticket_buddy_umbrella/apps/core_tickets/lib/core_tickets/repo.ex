defmodule CoreTickets.Repo do
  use Ecto.Repo,
    otp_app: :core_tickets,
    adapter: Ecto.Adapters.Postgres
end
