defmodule CoreTickets.TicketResource.Changes.ProcessReservation do
  @moduledoc """
  Change that transitions a ticket from :reserved to :confirmed.

  Adds a validation error if the ticket is not currently :reserved.
  Emits a telemetry event for observability.
  """
  use Ash.Resource.Change

  require Logger

  @impl true
  def change(changeset, _opts, _context) do
    current_status = Ash.Changeset.get_attribute(changeset, :status)

    cond do
      current_status && current_status != :reserved ->
        Ash.Changeset.add_error(changeset,
          field: :status,
          message: "can only process a ticket that is currently :reserved"
        )

      true ->
        changeset
        |> Ash.Changeset.force_change_attribute(:status, :confirmed)
        |> Ash.Changeset.before_action(fn cs ->
          data = Ash.Changeset.get_data(cs, :before_changes)
          :telemetry.execute(
            [:core_tickets, :ticket, :process_reservation, :start],
            %{count: 1},
            %{ticket_id: data && data.id}
          )
          cs
        end)
  |> Ash.Changeset.after_action(fn _cs, result ->
          :telemetry.execute(
            [:core_tickets, :ticket, :process_reservation, :stop],
            %{count: 1},
            %{ticket_id: result.id}
          )
          # Enqueue outbox message (best-effort; if it fails, surface error)
          case CoreTickets.Domain.enqueue_outbox_message(%{
                 topic: "tickets",
                 event_type: "ticket.confirmed",
                 payload: %{
                   ticket_id: result.id,
                   status: result.status,
                   event_id: result.event_id,
                   user_id: result.user_id,
                   price_cents: result.price_cents
                 }
               }) do
            {:ok, _msg} -> {:ok, result}
            {:error, error} -> {:error, error}
          end
        end)
    end
  end
end
