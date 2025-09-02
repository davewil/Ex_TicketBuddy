import {describe, it} from "vitest";
import {
    should_display_seat_map_for_event,
    should_navigate_back_to_events,
    should_allow_selecting_multiple_seats_and_proceed_to_purchase
} from "./Tickets.steps.ts";

describe("Tickets", () => {
    it("should display seat map for event", should_display_seat_map_for_event);
    it("should navigate back to events page when Back to Events button is clicked", should_navigate_back_to_events);
    it("should allow selecting multiple seats and proceed to purchase", should_allow_selecting_multiple_seats_and_proceed_to_purchase);
});
