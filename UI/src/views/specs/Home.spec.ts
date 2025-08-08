import {describe, it} from "vitest";
import {should_load_events_on_render} from "./Home.steps.ts";

describe("Home", () => {
    it("should load events on render", should_load_events_on_render);
});