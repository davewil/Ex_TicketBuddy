defmodule CoreEvents.Repo do
  use Ecto.Repo,
    otp_app: :core_events,
    adapter: Ecto.Adapters.Postgres
end
