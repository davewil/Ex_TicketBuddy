import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {eventFormIsRendered, formFieldIsRendered, renderEventsManagement, unmountEventsManagement} from "./EventsManagement.page.tsx";

const mockServer = MockServer.New();

beforeEach(() => {
    mockServer.reset();
    mockServer.start();
});

afterEach(() => {
    unmountEventsManagement();
});

export async function should_render_event_creation_form() {
    renderEventsManagement();
    expect(eventFormIsRendered()).toBeTruthy();
    expect(formFieldIsRendered("Event Name")).toBeTruthy();
    expect(formFieldIsRendered("Date")).toBeTruthy();
    expect(formFieldIsRendered("Venue")).toBeTruthy();
}