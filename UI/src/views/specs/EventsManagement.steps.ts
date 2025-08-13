import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {
    eventFormIsRendered,
    formFieldIsRendered,
    renderEventsManagement,
    unmountEventsManagement,
    fillEventForm,
    clickSubmitEventButton,
    formFieldIsReset
} from "./EventsManagement.page.tsx";
import { Venue } from "../../domain/event.ts";
import {waitUntil} from "../../testing/utilities.ts";

const mockServer = MockServer.New();
let wait_for_post: () => boolean;

beforeEach(() => {
    mockServer.reset();
    wait_for_post = mockServer.post("/events", {}, true, undefined);
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

export async function should_allow_user_to_create_new_event() {
    renderEventsManagement();
    expect(eventFormIsRendered()).toBeTruthy();

    const eventName = "Test Event";
    const eventDate = new Date();
    eventDate.setDate(eventDate.getDate() + 1);
    const eventDateString = eventDate.toISOString().split("T")[0];
    const eventDateStringWithTime = eventDate.toISOString().split("T")[0] + "T00:00:00.000Z";
    const eventVenue = Venue.O2ArenaLondon;

    await fillEventForm({
        eventName: eventName,
        date: eventDateString,
        venue: eventVenue
    });
    await clickSubmitEventButton();
    await waitUntil(wait_for_post);
    const data = mockServer.content
    expect(data).toEqual({
        EventName: eventName,
        Date: eventDateStringWithTime,
        Venue: eventVenue
    });
    expect(formFieldIsReset("Event Name")).toBeTruthy();
    expect(formFieldIsReset("Date")).toBeTruthy();
}