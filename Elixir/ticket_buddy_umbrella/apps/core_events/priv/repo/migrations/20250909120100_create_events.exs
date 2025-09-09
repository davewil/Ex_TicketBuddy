defmodule CoreEvents.Repo.Migrations.CreateEvents do
  use Ecto.Migration

  def change do
    create table(:events) do
      add :name, :text, null: false
      add :starts_at, :utc_datetime, null: false
      add :ends_at, :utc_datetime
      add :venue, :text
      timestamps(type: :utc_datetime)
    end

    create index(:events, [:starts_at])
  end
end
