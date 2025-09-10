defmodule CoreTickets.ListConfirmedTicketsTest do
  use CoreTickets.DataCase

  setup do
    {:ok, user} =
      Ash.create(CoreUsers.UserResource, %{
        name: "List User",
        email: "list@example.com"
      })

    {:ok, event} =
      Ash.create(CoreEvents.EventResource, %{
        name: "List Event",
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "List Venue"
      })

    {:ok, reserved} =
      CoreTickets.Domain.create_ticket(%{
        event_id: event.id,
        user_id: user.id,
        price_cents: 1000,
        status: :reserved
      })

    {:ok, confirmed} =
      CoreTickets.Domain.create_ticket(%{
        event_id: event.id,
        user_id: user.id,
        price_cents: 2000,
        status: :confirmed
      })

    %{reserved: reserved, confirmed: confirmed}
  end

  test "returns only confirmed tickets", %{reserved: reserved, confirmed: confirmed} do
    assert {:ok, tickets} = CoreTickets.Domain.list_confirmed_tickets()
    ids = Enum.map(tickets, & &1.id)
    assert confirmed.id in ids
    refute reserved.id in ids
  end
end
