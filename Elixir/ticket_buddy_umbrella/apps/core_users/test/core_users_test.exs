defmodule CoreUsersTest do
  use ExUnit.Case
  doctest CoreUsers

  test "greets the world" do
    assert CoreUsers.hello() == :world
  end
end
