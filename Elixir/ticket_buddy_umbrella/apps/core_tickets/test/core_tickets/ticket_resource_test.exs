defmodule CoreTickets.TicketResourceTest do
  use CoreTickets.DataCase

  setup do
    # Create user and event for ticket tests
    {:ok, user} =
      Ash.create(CoreUsers.UserResource, %{
        name: "Test User",
        email: "test@example.com"
      })

    {:ok, event} =
      Ash.create(CoreEvents.EventResource, %{
        name: "Test Event",
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "Test Venue"
      })

    %{user: user, event: event}
  end

  describe "ticket creation" do
    test "creates ticket with valid attributes", %{user: user, event: event} do
      ticket_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 2500,
        status: :reserved
      }

      assert {:ok, ticket} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
      assert ticket.event_id == event.id
      assert ticket.user_id == user.id
      assert ticket.price_cents == 2500
      assert ticket.status == :reserved
      assert ticket.id != nil
    end

    test "creates ticket with confirmed status", %{user: user, event: event} do
      ticket_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 5000,
        status: :confirmed
      }

      assert {:ok, ticket} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
      assert ticket.status == :confirmed
      assert ticket.price_cents == 5000
    end

    test "creates ticket with cancelled status", %{user: user, event: event} do
      ticket_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 1000,
        status: :cancelled
      }

      assert {:ok, ticket} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
      assert ticket.status == :cancelled
    end

    test "fails to create ticket with missing event_id", %{user: user} do
      ticket_attrs = %{
        user_id: user.id,
        price_cents: 2500,
        status: :reserved
      }

      assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
    end

    test "fails to create ticket with missing user_id", %{event: event} do
      ticket_attrs = %{
        event_id: event.id,
        price_cents: 2500,
        status: :reserved
      }

      assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
    end

    test "fails to create ticket with missing price", %{user: user, event: event} do
      ticket_attrs = %{
        event_id: event.id,
        user_id: user.id,
        status: :reserved
      }

      assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
    end
  end

  describe "ticket reading" do
    setup %{user: user, event: event} do
      {:ok, ticket} =
        Ash.create(CoreTickets.TicketResource, %{
          event_id: event.id,
          user_id: user.id,
          price_cents: 3000,
          status: :confirmed
        })

      %{ticket: ticket, user: user, event: event}
    end

    test "reads ticket by id", %{ticket: ticket} do
      assert {:ok, fetched_ticket} = Ash.get(CoreTickets.TicketResource, ticket.id)
      assert fetched_ticket.id == ticket.id
      assert fetched_ticket.price_cents == ticket.price_cents
      assert fetched_ticket.status == ticket.status
    end

    test "lists all tickets", %{ticket: ticket} do
      assert {:ok, tickets} = Ash.read(CoreTickets.TicketResource)
      assert is_list(tickets)
      assert length(tickets) >= 1
      assert Enum.any?(tickets, fn t -> t.id == ticket.id end)
    end

    test "reads non-existent ticket" do
      fake_id = Ecto.UUID.generate()
      assert {:error, error} = Ash.get(CoreTickets.TicketResource, fake_id)
      first_error = hd(error.errors)

      assert match?(%Ash.Error.Query.NotFound{}, first_error) or
               match?(%Ash.Error.Invalid.InvalidPrimaryKey{}, first_error)
    end
  end

  describe "ticket updating" do
    setup %{user: user, event: event} do
      {:ok, ticket} =
        Ash.create(CoreTickets.TicketResource, %{
          event_id: event.id,
          user_id: user.id,
          price_cents: 2000,
          status: :reserved
        })

      %{ticket: ticket, user: user, event: event}
    end

    test "updates ticket status", %{ticket: ticket} do
      assert {:ok, updated_ticket} = Ash.update(ticket, %{status: :confirmed})
      assert updated_ticket.status == :confirmed
      assert updated_ticket.price_cents == ticket.price_cents
    end

    test "updates ticket price", %{ticket: ticket} do
      assert {:ok, updated_ticket} = Ash.update(ticket, %{price_cents: 3500})
      assert updated_ticket.price_cents == 3500
      assert updated_ticket.status == ticket.status
    end

    test "updates multiple fields", %{ticket: ticket} do
      assert {:ok, updated_ticket} =
               Ash.update(ticket, %{
                 status: :cancelled,
                 price_cents: 0
               })

      assert updated_ticket.status == :cancelled
      assert updated_ticket.price_cents == 0
    end
  end

  describe "ticket deletion" do
    setup %{user: user, event: event} do
      {:ok, ticket} =
        Ash.create(CoreTickets.TicketResource, %{
          event_id: event.id,
          user_id: user.id,
          price_cents: 2500,
          status: :reserved
        })

      %{ticket: ticket, user: user, event: event}
    end

    test "deletes ticket", %{ticket: ticket} do
      assert :ok = Ash.destroy(ticket)
      assert {:error, error} = Ash.get(CoreTickets.TicketResource, ticket.id)
      assert match?(%Ash.Error.Query.NotFound{}, hd(error.errors))
    end
  end

  describe "ticket business logic" do
    test "allows multiple tickets for same event and user", %{user: user, event: event} do
      ticket1_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 2500,
        status: :reserved
      }

      ticket2_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 2500,
        status: :confirmed
      }

      assert {:ok, ticket1} = Ash.create(CoreTickets.TicketResource, ticket1_attrs)
      assert {:ok, ticket2} = Ash.create(CoreTickets.TicketResource, ticket2_attrs)
      assert ticket1.id != ticket2.id
    end

    test "allows different prices for same event", %{user: user, event: event} do
      # Create another user
      {:ok, user2} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Second User",
          email: "second@example.com"
        })

      ticket1_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 2500,
        status: :reserved
      }

      ticket2_attrs = %{
        event_id: event.id,
        user_id: user2.id,
        # Different price
        price_cents: 1500,
        status: :confirmed
      }

      assert {:ok, ticket1} = Ash.create(CoreTickets.TicketResource, ticket1_attrs)
      assert {:ok, ticket2} = Ash.create(CoreTickets.TicketResource, ticket2_attrs)
      assert ticket1.price_cents != ticket2.price_cents
    end

    test "handles zero price tickets", %{user: user, event: event} do
      ticket_attrs = %{
        event_id: event.id,
        user_id: user.id,
        price_cents: 0,
        status: :confirmed
      }

      assert {:ok, ticket} = Ash.create(CoreTickets.TicketResource, ticket_attrs)
      assert ticket.price_cents == 0
    end
  end
end
