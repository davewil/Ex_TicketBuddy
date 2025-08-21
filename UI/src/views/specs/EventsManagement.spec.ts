import {describe, it} from "vitest";
import {
    should_render_event_creation_form,
    should_allow_user_to_create_new_event, should_display_list_of_events
} from "./EventsManagement.steps.ts";

describe("Events Management", () => {
    it('should display list of events', should_display_list_of_events);
    it("should render event creation form when add event icon clicked", should_render_event_creation_form);
    it('should allow user to create a new event', should_allow_user_to_create_new_event);
});
