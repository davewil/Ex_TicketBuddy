defmodule CoreTickets.Repo.Migrations.CreateTicketOutboxMessages do
  use Ecto.Migration

  @doc """
  Legacy manual migration retained only to avoid conflicts; table now created by Ash-generated migration.
  Made idempotent so re-running does not fail if table already exists.
  """
  def change do
    execute "CREATE TABLE IF NOT EXISTS ticket_outbox_messages (\n  id BIGSERIAL PRIMARY KEY,\n  topic text NOT NULL,\n  event_type text NOT NULL,\n  payload jsonb NOT NULL,\n  published_at timestamptz,\n  inserted_at timestamptz NOT NULL DEFAULT (now() AT TIME ZONE 'utc')\n)"

    execute "CREATE INDEX IF NOT EXISTS ticket_outbox_messages_published_at_idx ON ticket_outbox_messages (published_at)"
    execute "CREATE INDEX IF NOT EXISTS ticket_outbox_messages_topic_idx ON ticket_outbox_messages (topic)"
  end
end
