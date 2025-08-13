import {describe, it} from "vitest";
import {should_render_event_creation_form} from "./EventsManagement.steps.ts";

describe("EventsManagement", () => {
    it("should render event creation form", should_render_event_creation_form);
});
