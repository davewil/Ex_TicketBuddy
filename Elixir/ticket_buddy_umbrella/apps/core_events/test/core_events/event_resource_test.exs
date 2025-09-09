defmodule CoreEvents.EventResourceTest do
  use CoreEvents.DataCase

  describe "event creation" do
    test "creates event with valid attributes" do
      event_attrs = %{
        name: "Elixir Meetup",
        starts_at: ~U[2025-12-01 18:00:00Z],
        ends_at: ~U[2025-12-01 21:00:00Z],
        venue: "Tech Hub"
      }

  assert {:ok, event} = Ash.create(CoreEvents.EventResource, event_attrs)
      assert event.name == "Elixir Meetup"
      assert event.starts_at == ~U[2025-12-01 18:00:00Z]
      assert event.ends_at == ~U[2025-12-01 21:00:00Z]
      assert event.venue == "Tech Hub"
      assert event.id != nil
    end

    test "creates event without venue" do
      event_attrs = %{
        name: "Online Workshop",
        starts_at: ~U[2025-12-15 14:00:00Z],
        ends_at: ~U[2025-12-15 17:00:00Z]
      }

  assert {:ok, event} = Ash.create(CoreEvents.EventResource, event_attrs)
      assert event.name == "Online Workshop"
      assert event.venue == nil
    end

    test "creates event without end time" do
      event_attrs = %{
        name: "Open Ended Event",
        starts_at: ~U[2025-12-20 10:00:00Z],
        venue: "Community Center"
      }

  assert {:ok, event} = Ash.create(CoreEvents.EventResource, event_attrs)
      assert event.name == "Open Ended Event"
      assert event.ends_at == nil
    end

    test "fails to create event with missing name" do
      event_attrs = %{
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "Tech Hub"
      }

  assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreEvents.EventResource, event_attrs)
    end

    test "fails to create event with missing start time" do
      event_attrs = %{
        name: "Invalid Event",
        venue: "Some Venue"
      }

  assert {:error, %Ash.Error.Invalid{}} = Ash.create(CoreEvents.EventResource, event_attrs)
    end
  end

  describe "event reading" do
    setup do
  {:ok, event} = Ash.create(CoreEvents.EventResource, %{
        name: "Test Conference",
        starts_at: ~U[2025-11-01 09:00:00Z],
        ends_at: ~U[2025-11-01 17:00:00Z],
        venue: "Convention Center"
      })
      %{event: event}
    end

    test "reads event by id", %{event: event} do
  assert {:ok, fetched_event} = Ash.get(CoreEvents.EventResource, event.id)
      assert fetched_event.id == event.id
      assert fetched_event.name == event.name
      assert fetched_event.venue == event.venue
    end

    test "lists all events", %{event: event} do
  assert {:ok, events} = Ash.read(CoreEvents.EventResource)
      assert is_list(events)
      assert length(events) >= 1
      assert Enum.any?(events, fn e -> e.id == event.id end)
    end
  end

  describe "event updating" do
    setup do
  {:ok, event} = Ash.create(CoreEvents.EventResource, %{
        name: "Original Event",
        starts_at: ~U[2025-12-01 10:00:00Z],
        venue: "Original Venue"
      })
      %{event: event}
    end

    test "updates event name", %{event: event} do
  assert {:ok, updated_event} = Ash.update(event, %{name: "Updated Event"})
      assert updated_event.name == "Updated Event"
      assert updated_event.venue == event.venue
    end

    test "updates event venue", %{event: event} do
  assert {:ok, updated_event} = Ash.update(event, %{venue: "New Venue"})
      assert updated_event.venue == "New Venue"
      assert updated_event.name == event.name
    end

    test "updates event times", %{event: event} do
      new_start = ~U[2025-12-02 14:00:00Z]
      new_end = ~U[2025-12-02 18:00:00Z]

  assert {:ok, updated_event} = Ash.update(event, %{
        starts_at: new_start,
        ends_at: new_end
      })
      assert updated_event.starts_at == new_start
      assert updated_event.ends_at == new_end
    end
  end

  describe "event deletion" do
    setup do
  {:ok, event} = Ash.create(CoreEvents.EventResource, %{
        name: "Event to Delete",
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "Delete Venue"
      })
      %{event: event}
    end

    test "deletes event", %{event: event} do
      assert :ok = Ash.destroy(event)
      assert {:error, %Ash.Error.Invalid{errors: [%Ash.Error.Query.NotFound{} | _]}} =
               Ash.get(CoreEvents.EventResource, event.id)
    end
  end

  describe "event business logic" do
    test "handles events with same name but different times" do
      event1_attrs = %{
        name: "Weekly Meetup",
        starts_at: ~U[2025-12-01 18:00:00Z],
        venue: "Tech Hub"
      }

      event2_attrs = %{
        name: "Weekly Meetup",
        starts_at: ~U[2025-12-08 18:00:00Z],
        venue: "Tech Hub"
      }

  assert {:ok, event1} = Ash.create(CoreEvents.EventResource, event1_attrs)
  assert {:ok, event2} = Ash.create(CoreEvents.EventResource, event2_attrs)
      assert event1.id != event2.id
      assert event1.name == event2.name
    end
  end
end
