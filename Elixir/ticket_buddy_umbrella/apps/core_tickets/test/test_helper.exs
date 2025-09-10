# Ensure test database is created & migrated before tests start
{:ok, _} = Application.ensure_all_started(:ecto_sql)
Mix.Task.run("ecto.create", ["-r", "CoreTickets.Repo", "--quiet"])
Mix.Task.run("ecto.migrate", ["-r", "CoreTickets.Repo", "--quiet"])

ExUnit.start()
Ecto.Adapters.SQL.Sandbox.mode(CoreTickets.Repo, :manual)
