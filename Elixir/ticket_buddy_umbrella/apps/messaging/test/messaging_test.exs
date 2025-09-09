defmodule MessagingTest do
  use ExUnit.Case
  doctest Messaging

  test "greets the world" do
    assert Messaging.hello() == :world
  end
end
