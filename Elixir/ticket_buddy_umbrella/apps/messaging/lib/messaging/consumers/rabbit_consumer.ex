defmodule Messaging.Consumers.RabbitConsumer do
  @moduledoc """
  Minimal Broadway consumer for RabbitMQ. Disabled by default; enable with config.
  """
  use Broadway

  require Logger

  def start_link(_opts) do
    if Application.get_env(:messaging, :enable_broadway, false) do
      Broadway.start_link(__MODULE__, pipeline_options())
    else
      # Start a dummy process that does nothing when disabled
      Task.start_link(fn -> :ok end)
    end
  end

  defp pipeline_options do
    queue = Application.get_env(:messaging, :rabbitmq_queue, "ticketbuddy.dev")
    uri = Application.get_env(:messaging, :rabbitmq_uri, "amqp://guest:guest@localhost:5672")

    [
      name: __MODULE__,
      producers: [
        default: [
          module: {BroadwayRabbitMQ.Producer, queue: queue, connection: [url: uri]},
          concurrency: 1
        ]
      ],
      processors: [default: [concurrency: 2]],
      batchers: [default: [concurrency: 2, batch_size: 10, batch_timeout: 1000]]
    ]
  end

  @impl true
  def handle_message(_proc, message, _ctx) do
    Logger.info("RabbitConsumer message: #{inspect(message.data)}")
    message
  end

  @impl true
  def handle_batch(_batcher, messages, _batch_info, _context) do
    messages
  end
end
