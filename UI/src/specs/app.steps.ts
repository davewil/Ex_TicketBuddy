import {homePageIsRendered, renderApp, unmountApp, userIconIsRendered, usersDropdownIsRendered} from "./app.page.tsx";
import {afterEach, beforeEach, expect} from "vitest";
import {MockServer} from "../testing/mock-server.ts";
import {Users} from "../testing/data.ts";
import {waitUntil} from "../testing/utilities.ts";

const mockServer = MockServer.New();
let wait_for_get: () => boolean;

beforeEach(() => {
    mockServer.reset();
    wait_for_get = mockServer.get("users", Users)
    mockServer.start();
});

afterEach(() => {
    unmountApp();
});

export function should_default_to_home_page() {
    renderApp();
    expect(homePageIsRendered()).toBeTruthy();
}

export async function should_load_list_of_users_to_select_from() {
    renderApp();
    await waitUntil(wait_for_get);
    expect(usersDropdownIsRendered()).toBeTruthy();
}

export async function should_show_user_icon_when_selected() {
    renderApp();
    await waitUntil(wait_for_get);
    expect(userIconIsRendered()).toBeTruthy();
}