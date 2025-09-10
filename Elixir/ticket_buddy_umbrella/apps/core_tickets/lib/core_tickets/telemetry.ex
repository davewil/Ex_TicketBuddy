defmodule CoreTickets.Telemetry do
  @moduledoc """
  Simple telemetry handler attachment for ticket reservation processing events.
  In a fuller system this would live in a shared telemetry app; placed here
  for local observability of the new action.
  """

  require Logger

  @events [
    [:core_tickets, :ticket, :process_reservation, :start],
    [:core_tickets, :ticket, :process_reservation, :stop]
  ]

  def attach do
    Enum.each(@events, fn event ->
      try do
        :telemetry.attach(handler_id(event), event, &__MODULE__.handle_event/4, %{})
      rescue
        _ -> :ok
      end
    end)
    :ok
  end

  defp handler_id(event), do: "core_tickets-" <> Enum.join(Enum.map(event, &to_string/1), "-")

  def handle_event([:core_tickets, :ticket, :process_reservation, stage] = event, measurements, metadata, _config) do
    Logger.debug("telemetry #{inspect(event)} stage=#{stage} measurements=#{inspect(measurements)} metadata=#{inspect(metadata)}")
  end
end
