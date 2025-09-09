alias CoreEvents.Domain
alias CoreEvents.EventResource
alias CoreUsers.Domain, as: UsersDomain
alias CoreUsers.UserResource
alias CoreTickets.Domain, as: TicketsDomain
alias CoreTickets.TicketResource

import Ecto.Query

# Create sample users
users = [
  %{name: "Alice Johnson", email: "alice@example.com"},
  %{name: "Bob Smith", email: "bob@example.com"},
  %{name: "Charlie Brown", email: "charlie@example.com"}
]

created_users = Enum.map(users, fn user_attrs ->
  case Ash.create(UserResource, user_attrs, domain: UsersDomain) do
    {:ok, user} ->
      IO.puts("Created user: #{user.name} (#{user.email})")
      user
    {:error, error} ->
      IO.puts("Failed to create user #{user_attrs.name}: #{inspect(error)}")
      nil
  end
end)
|> Enum.filter(&(&1 != nil))

# Create sample events
events = [
  %{
    name: "Elixir Conference 2025",
    starts_at: ~U[2025-10-15 09:00:00Z],
    ends_at: ~U[2025-10-15 17:00:00Z],
    venue: "Convention Center"
  },
  %{
    name: "Phoenix LiveView Workshop",
    starts_at: ~U[2025-11-01 10:00:00Z],
    ends_at: ~U[2025-11-01 16:00:00Z],
    venue: "Tech Hub"
  },
  %{
    name: "OTP Deep Dive",
    starts_at: ~U[2025-11-20 14:00:00Z],
    ends_at: ~U[2025-11-20 18:00:00Z],
    venue: "Online"
  }
]

created_events = Enum.map(events, fn event_attrs ->
  case Ash.create(EventResource, event_attrs, domain: Domain) do
    {:ok, event} ->
      IO.puts("Created event: #{event.name}")
      event
    {:error, error} ->
      IO.puts("Failed to create event #{event_attrs.name}: #{inspect(error)}")
      nil
  end
end)
|> Enum.filter(&(&1 != nil))

# Create sample tickets
if length(created_users) > 0 and length(created_events) > 0 do
  first_user = List.first(created_users)
  second_user = Enum.at(created_users, 1, first_user)
  first_event = List.first(created_events)
  second_event = Enum.at(created_events, 1, first_event)

  tickets = [
    %{
      user_id: first_user.id,
      event_id: first_event.id,
      price_cents: 15000,
      status: :purchased
    },
    %{
      user_id: second_user.id,
      event_id: first_event.id,
      price_cents: 15000,
      status: :reserved
    },
    %{
      user_id: first_user.id,
      event_id: second_event.id,
      price_cents: 8500,
      status: :purchased
    }
  ]

  Enum.each(tickets, fn ticket_attrs ->
    case Ash.create(TicketResource, ticket_attrs, domain: TicketsDomain) do
      {:ok, ticket} ->
        IO.puts("Created ticket for user #{ticket.user_id}, event #{ticket.event_id} - $#{ticket.price_cents/100}")
      {:error, error} ->
        IO.puts("Failed to create ticket: #{inspect(error)}")
    end
  end)
end

IO.puts("\n✅ Seed data creation completed!")
IO.puts("📊 Summary:")
IO.puts("   - Users: #{length(created_users)}")
IO.puts("   - Events: #{length(created_events)}")
