IO.puts("Starting core apps and messaging (Oban)...")

{:ok, _} = Application.ensure_all_started(:core_users)
{:ok, _} = Application.ensure_all_started(:core_events)
{:ok, _} = Application.ensure_all_started(:core_tickets)
{:ok, _} = Application.ensure_all_started(:messaging)

alias CoreTickets.TicketResource

params = %{event_id: 1, user_id: 1, price_cents: 2000, status: :reserved}

ticket = Ash.create!(TicketResource, params)

IO.puts("Created reserved ticket id=#{ticket.id}. Waiting ~70s for AshOban scheduler...")

:timer.sleep(70_000)

IO.puts("Listing recent oban jobs for queue=ticket_resource_process:")
Code.require_file("priv/demo/list_oban_jobs.exs")
