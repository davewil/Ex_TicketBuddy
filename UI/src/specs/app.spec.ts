import {describe, it} from "vitest";
import {should_default_to_home_page, should_load_list_of_users_to_select_from} from "./app.steps.ts";

describe('App', () => {
    it('should default to the home page', should_default_to_home_page);
    it('should load list of users to select from', should_load_list_of_users_to_select_from);
});