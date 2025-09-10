defmodule CoreTickets.ProcessReservationActionTest do
  use CoreTickets.DataCase

  describe "process_reservation action" do
    setup do
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Proc User",
            email: "proc@example.com"
        })

      {:ok, event} =
        Ash.create(CoreEvents.EventResource, %{
          name: "Proc Event",
          starts_at: ~U[2025-12-01 18:00:00Z],
          venue: "Proc Venue"
        })

      {:ok, ticket} =
        CoreTickets.Domain.create_ticket(%{
          event_id: event.id,
          user_id: user.id,
          price_cents: 1234,
          status: :reserved
        })

      %{ticket: ticket, user: user, event: event}
    end

    test "transitions reserved -> confirmed", %{ticket: ticket} do
      assert {:ok, processed} = CoreTickets.Domain.process_reservation(ticket)
      assert processed.status == :confirmed
    end

    test "fails when ticket not reserved", %{ticket: ticket} do
      # First process
      assert {:ok, processed} = CoreTickets.Domain.process_reservation(ticket)
      assert processed.status == :confirmed
      # Second attempt should error
      assert {:error, %Ash.Error.Invalid{} = err} = CoreTickets.Domain.process_reservation(processed)
      assert Enum.any?(err.errors, fn e -> to_string(e.message) =~ "only process" end)
    end
  end
end
