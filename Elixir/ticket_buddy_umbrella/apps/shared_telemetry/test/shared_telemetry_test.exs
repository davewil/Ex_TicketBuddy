defmodule SharedTelemetryTest do
  use ExUnit.Case
  doctest SharedTelemetry

  test "greets the world" do
    assert SharedTelemetry.hello() == :world
  end
end
