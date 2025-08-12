import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach} from "vitest";
import {Events, Users} from "../../testing/data.ts";
import {eventExists, renderHome, unmountHome} from "./Home.page.tsx";
import {waitUntil} from "../../testing/utilities.ts";
import {expect} from "vitest";

const mockServer = MockServer.New();
let wait_for_get_events: () => boolean;
let wait_for_get_users: () => boolean;

beforeEach(() => {
    mockServer.reset();
    wait_for_get_events = mockServer.get("events", Events)
    wait_for_get_users = mockServer.get("users", Users);
    mockServer.start();
});

afterEach(() => {
    unmountHome();
});

export async function should_load_events_on_render() {
    renderHome();
    await waitUntil(wait_for_get_events);
    await waitUntil(wait_for_get_users);
    for (const event of Events) {
        expect(eventExists(event.EventName)).toBeTruthy();
    }
}