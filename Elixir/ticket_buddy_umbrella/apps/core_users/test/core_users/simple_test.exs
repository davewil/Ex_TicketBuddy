defmodule CoreUsers.SimpleTest do
  use CoreUsers.DataCase

  test "creates user with Ash.create" do
    user_attrs = %{
      name: "Test User",
      email: "test@example.com"
    }

    assert {:ok, user} = Ash.create(CoreUsers.UserResource, user_attrs)
    assert user.name == "Test User"
    assert user.email == "test@example.com"
    assert user.id != nil
  end

  test "reads user with Ash.get" do
    {:ok, user} = Ash.create(CoreUsers.UserResource, %{
      name: "Test User",
      email: "test@example.com"
    })

    assert {:ok, fetched_user} = Ash.get(CoreUsers.UserResource, user.id)
    assert fetched_user.id == user.id
  end
end
