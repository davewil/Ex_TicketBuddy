defmodule CoreUsers.UserResourceTest do
  use CoreUsers.DataCase

  describe "user creation" do
    test "creates user with valid attributes" do
      user_attrs = %{
        name: "John Doe",
        email: "john@example.com"
      }

      assert {:ok, user} = Ash.create(CoreUsers.UserResource, user_attrs)
      assert user.name == "John Doe"
      assert user.email == "john@example.com"
      assert user.id != nil
    end

    test "fails to create user with missing name" do
      user_attrs = %{
        email: "john@example.com"
      }

      assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreUsers.UserResource, user_attrs)
    end

    test "fails to create user with missing email" do
      user_attrs = %{
        name: "John Doe"
      }

      assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreUsers.UserResource, user_attrs)
    end
  end

  describe "user reading" do
    setup do
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Jane Doe",
          email: "jane@example.com"
        })

      %{user: user}
    end

    test "reads user by id", %{user: user} do
      assert {:ok, fetched_user} = Ash.get(CoreUsers.UserResource, user.id)
      assert fetched_user.id == user.id
      assert fetched_user.name == user.name
      assert fetched_user.email == user.email
    end

    test "lists all users", %{user: user} do
      assert {:ok, users} = Ash.read(CoreUsers.UserResource)
      assert is_list(users)
      assert length(users) >= 1
      assert Enum.any?(users, fn u -> u.id == user.id end)
    end

    test "returns error for non-existent user" do
      assert {:error, error} = Ash.get(CoreUsers.UserResource, 999_999)
      assert match?(%Ash.Error.Query.NotFound{}, hd(error.errors))
    end
  end

  describe "user updating" do
    setup do
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Original Name",
          email: "original@example.com"
        })

      %{user: user}
    end

    test "updates user name", %{user: user} do
      assert {:ok, updated_user} = Ash.update(user, %{name: "Updated Name"})
      assert updated_user.name == "Updated Name"
      assert updated_user.email == user.email
      assert updated_user.id == user.id
    end

    test "updates user email", %{user: user} do
      assert {:ok, updated_user} = Ash.update(user, %{email: "updated@example.com"})
      assert updated_user.email == "updated@example.com"
      assert updated_user.name == user.name
    end
  end

  describe "user deletion" do
    setup do
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "To Delete",
          email: "delete@example.com"
        })

      %{user: user}
    end

    test "deletes user", %{user: user} do
      assert :ok = Ash.destroy(user)
      assert {:error, error} = Ash.get(CoreUsers.UserResource, user.id)
      assert match?(%Ash.Error.Query.NotFound{}, hd(error.errors))
    end
  end
end
