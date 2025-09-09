defmodule CoreUsers.Repo do
  use Ecto.Repo,
    otp_app: :core_users,
    adapter: Ecto.Adapters.Postgres
end
