defmodule Messaging.Workers.DemoWorker do
  @moduledoc """
  A no-op Oban worker used to validate Oban wiring.
  """
  use Oban.Worker, queue: :default, max_attempts: 3

  @impl true
  def perform(%Oban.Job{args: args}) do
    require Logger
    Logger.info("DemoWorker executed with args=#{inspect(args)}")
    :ok
  end
end
