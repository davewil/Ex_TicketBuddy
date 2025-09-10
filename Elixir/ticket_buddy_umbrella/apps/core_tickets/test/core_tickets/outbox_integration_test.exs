defmodule CoreTickets.OutboxIntegrationTest do
  use CoreTickets.DataCase

  setup do
    {:ok, user} =
      Ash.create(CoreUsers.UserResource, %{
        name: "Outbox User",
        email: "outbox@example.com"
      })

    {:ok, event} =
      Ash.create(CoreEvents.EventResource, %{
        name: "Outbox Event",
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "Outbox Venue"
      })

    {:ok, ticket} =
      CoreTickets.Domain.create_ticket(%{
        event_id: event.id,
        user_id: user.id,
        price_cents: 777,
        status: :reserved
      })

    %{ticket: ticket}
  end

  test "processing reservation enqueues outbox message", %{ticket: ticket} do
    assert {:ok, processed} = CoreTickets.Domain.process_reservation(ticket)
    assert processed.status == :confirmed

    assert {:ok, messages} = CoreTickets.Domain.list_outbox_messages()
    assert Enum.any?(messages, fn m -> m.event_type == "ticket.confirmed" and m.payload["ticket_id"] == processed.id end)
  end
end
