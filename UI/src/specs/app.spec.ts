import {describe, it} from "vitest";
import {should_default_to_home_page} from "./app.steps.ts";

describe('App', () => {
    it('should default to the home page', should_default_to_home_page);
});