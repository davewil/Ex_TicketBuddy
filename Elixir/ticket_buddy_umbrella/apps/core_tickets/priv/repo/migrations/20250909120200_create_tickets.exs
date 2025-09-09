defmodule CoreTickets.Repo.Migrations.CreateTickets do
  use Ecto.Migration

  def change do
    create table(:tickets) do
      add :event_id, :bigint, null: false
      add :user_id, :bigint, null: false
      add :price_cents, :integer, null: false
      add :status, :string, null: false, default: "reserved"
      timestamps(type: :utc_datetime)
    end

    create index(:tickets, [:event_id])
    create index(:tickets, [:user_id])
    create constraint(:tickets, :price_cents_non_negative, check: "price_cents >= 0")
  end
end
