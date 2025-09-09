defmodule CoreEventsTest do
  use ExUnit.Case
  doctest CoreEvents

  test "greets the world" do
    assert CoreEvents.hello() == :world
  end
end
