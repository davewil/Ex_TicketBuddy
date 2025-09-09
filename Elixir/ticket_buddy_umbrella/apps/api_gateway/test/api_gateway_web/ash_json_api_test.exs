defmodule ApiGatewayWeb.AshJsonApiTest do
  use ApiGatewayWeb.ConnCase

  import Plug.Conn

  describe "Users API" do
    setup do
      # Create test user
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Test User",
          email: "test@example.com"
        })

      %{user: user}
    end

    test "GET /api/users returns list of users", %{conn: conn} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/users")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert is_map(response)
      assert Map.has_key?(response, "data")
      assert is_list(response["data"])
    end

    test "GET /api/users/:id returns a specific user", %{conn: conn, user: user} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/users/#{user.id}")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert response["data"]["id"] == to_string(user.id)
      assert response["data"]["attributes"]["name"] == user.name
      assert response["data"]["attributes"]["email"] == user.email
    end

    test "POST /api/users creates a new user", %{conn: conn} do
      user_data = %{
        "data" => %{
          "type" => "users",
          "attributes" => %{
            "name" => "New User",
            "email" => "new@example.com"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> post("/api/users", user_data)

      assert json_response(conn, 201)
      response = json_response(conn, 201)
      assert response["data"]["attributes"]["name"] == "New User"
      assert response["data"]["attributes"]["email"] == "new@example.com"
    end

    test "PATCH /api/users/:id updates a user", %{conn: conn, user: user} do
      update_data = %{
        "data" => %{
          "type" => "users",
          "id" => to_string(user.id),
          "attributes" => %{
            "name" => "Updated User"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> patch("/api/users/#{user.id}", update_data)

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert response["data"]["attributes"]["name"] == "Updated User"
      assert response["data"]["attributes"]["email"] == user.email
    end

    test "DELETE /api/users/:id deletes a user", %{conn: conn, user: user} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> delete("/api/users/#{user.id}")

      assert json_response(conn, 200)
      # Optionally verify structure
      resp = json_response(conn, 200)
      assert resp["data"]["type"] == "users"
    end
  end

  describe "Events API" do
    setup do
      # Create test event
      {:ok, event} =
        Ash.create(CoreEvents.EventResource, %{
          name: "Test Event",
          starts_at: ~U[2025-12-01 10:00:00Z],
          ends_at: ~U[2025-12-01 18:00:00Z],
          venue: "Test Venue"
        })

      %{event: event}
    end

    test "GET /api/events returns list of events", %{conn: conn} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/events")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert is_map(response)
      assert Map.has_key?(response, "data")
      assert is_list(response["data"])
    end

    test "GET /api/events/:id returns a specific event", %{conn: conn, event: event} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/events/#{event.id}")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert response["data"]["id"] == to_string(event.id)
      assert response["data"]["attributes"]["name"] == event.name
      assert response["data"]["attributes"]["venue"] == event.venue
    end

    test "POST /api/events creates a new event", %{conn: conn} do
      event_data = %{
        "data" => %{
          "type" => "events",
          "attributes" => %{
            "name" => "New Event",
            "starts_at" => "2025-12-15T10:00:00Z",
            "ends_at" => "2025-12-15T18:00:00Z",
            "venue" => "New Venue"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> post("/api/events", event_data)

      assert json_response(conn, 201)
      response = json_response(conn, 201)
      assert response["data"]["attributes"]["name"] == "New Event"
      assert response["data"]["attributes"]["venue"] == "New Venue"
    end
  end

  describe "Tickets API" do
    setup do
      # Create test user and event
      {:ok, user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "Ticket User",
          email: "ticket@example.com"
        })

      {:ok, event} =
        Ash.create(CoreEvents.EventResource, %{
          name: "Ticket Event",
          starts_at: ~U[2025-12-01 10:00:00Z],
          ends_at: ~U[2025-12-01 18:00:00Z],
          venue: "Ticket Venue"
        })

      # Create test ticket
      {:ok, ticket} =
        Ash.create(CoreTickets.TicketResource, %{
          user_id: user.id,
          event_id: event.id,
          price_cents: 5000,
          status: :reserved
        })

      %{user: user, event: event, ticket: ticket}
    end

    test "GET /api/tickets returns list of tickets", %{conn: conn} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/tickets")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert is_map(response)
      assert Map.has_key?(response, "data")
      assert is_list(response["data"])
    end

    test "GET /api/tickets/:id returns a specific ticket", %{conn: conn, ticket: ticket} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/tickets/#{ticket.id}")

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert response["data"]["id"] == to_string(ticket.id)
      assert response["data"]["attributes"]["price_cents"] == ticket.price_cents
      assert response["data"]["attributes"]["status"] == to_string(ticket.status)
    end

    test "POST /api/tickets creates a new ticket", %{conn: conn, user: user, event: event} do
      ticket_data = %{
        "data" => %{
          "type" => "tickets",
          "attributes" => %{
            "user_id" => user.id,
            "event_id" => event.id,
            "price_cents" => 7500,
            "status" => "purchased"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> post("/api/tickets", ticket_data)

      assert json_response(conn, 201)
      response = json_response(conn, 201)
      assert response["data"]["attributes"]["price_cents"] == 7500
      assert response["data"]["attributes"]["status"] == "purchased"
    end

    test "PATCH /api/tickets/:id updates a ticket status", %{conn: conn, ticket: ticket} do
      update_data = %{
        "data" => %{
          "type" => "tickets",
          "id" => to_string(ticket.id),
          "attributes" => %{
            "status" => "purchased"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> patch("/api/tickets/#{ticket.id}", update_data)

      assert json_response(conn, 200)
      response = json_response(conn, 200)
      assert response["data"]["attributes"]["status"] == "purchased"
    end
  end

  describe "Error handling" do
    test "GET /api/users/999999 returns 404 for non-existent user", %{conn: conn} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/users/999999")

      assert json_response(conn, 404)
      response = json_response(conn, 404)
      assert Map.has_key?(response, "errors")
    end

    test "POST /api/users with invalid data returns 400", %{conn: conn} do
      invalid_data = %{
        "data" => %{
          "type" => "users",
          "attributes" => %{
            # Invalid: empty name
            "name" => "",
            # Invalid: bad email format
            "email" => "not-an-email"
          }
        }
      }

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> put_req_header("content-type", "application/vnd.api+json")
        |> post("/api/users", invalid_data)

      assert json_response(conn, 400)
      response = json_response(conn, 400)
      assert Map.has_key?(response, "errors")
    end

    test "requests without proper headers return 200", %{conn: conn} do
      conn =
        conn
        |> get("/api/users")

      assert response(conn, 200)
    end
  end

  describe "JSON:API compliance" do
    test "responses have correct JSON:API structure", %{conn: conn} do
      # Create a test user first
      {:ok, _user} =
        Ash.create(CoreUsers.UserResource, %{
          name: "JSON API User",
          email: "jsonapi@example.com"
        })

      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/users")

      response = json_response(conn, 200)

      # Check JSON:API structure
      assert Map.has_key?(response, "data")
      assert is_list(response["data"])

      if length(response["data"]) > 0 do
        user = hd(response["data"])
        assert Map.has_key?(user, "id")
        assert Map.has_key?(user, "type")
        assert Map.has_key?(user, "attributes")
        assert user["type"] == "users"
      end
    end

    test "content-type header is set correctly", %{conn: conn} do
      conn =
        conn
        |> put_req_header("accept", "application/vnd.api+json")
        |> get("/api/users")

      assert response(conn, 200)
      content_type = conn |> get_resp_header("content-type") |> hd()
      assert String.contains?(content_type, "application/vnd.api+json")
    end
  end
end
