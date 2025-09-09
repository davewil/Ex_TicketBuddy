defmodule CoreEvents.Repo do
  use AshPostgres.Repo, otp_app: :core_events

  def installed_extensions do
    ["citext", "uuid-ossp", "ash-functions"]
  end

  def min_pg_version do
    %Version{major: 14, minor: 0, patch: 0}
  end
end
