alias CoreTickets.TicketResource

params = %{event_id: 1, user_id: 1, price_cents: 1000, status: :reserved}

ticket = Ash.create!(TicketResource, params)

IO.puts("Created reserved ticket id=#{ticket.id}; AshOban trigger will schedule within ~1 minute.")
