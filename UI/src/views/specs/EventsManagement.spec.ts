import {describe, it} from "vitest";
import {
    should_render_event_creation_form,
    should_allow_user_to_create_new_event
} from "./EventsManagement.steps.ts";

describe("EventsManagement", () => {
    it("should render event creation form", should_render_event_creation_form);
    it('should allow user to create a new event', should_allow_user_to_create_new_event);
});
