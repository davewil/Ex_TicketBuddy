import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {
    eventFormIsRendered,
    formFieldIsRendered,
    renderEventsManagement,
    unmountEventsManagement,
    fillEventForm,
    clickSubmitEventButton,
    clickAddEventIcon,
    eventExists,
    backButtonIsRendered,
    clickBackButton
} from "./EventsManagement.page.tsx";
import { Venue } from "../../domain/event.ts";
import {waitUntil} from "../../testing/utilities.ts";
import {Events} from "../../testing/data.ts";

const mockServer = MockServer.New();
let wait_for_post: () => boolean;
let wait_for_get_events: () => boolean;

beforeEach(() => {
    mockServer.reset();
    wait_for_get_events = mockServer.get("/events", Events);
    wait_for_post = mockServer.post("/events", {}, true, undefined);
    mockServer.start();
});

afterEach(() => {
    unmountEventsManagement();
});

export async function should_display_list_of_events() {
    renderEventsManagement();
    await waitUntil(wait_for_get_events);
    for (const event of Events) {
        expect(eventExists(event.EventName)).toBeTruthy();
    }
}

export async function should_render_event_creation_form() {
    renderEventsManagement();
    await clickAddEventIcon();
    expect(eventFormIsRendered()).toBeTruthy();
    expect(formFieldIsRendered("Event Name")).toBeTruthy();
    expect(formFieldIsRendered("Start Date")).toBeTruthy();
    expect(formFieldIsRendered("End Date")).toBeTruthy();
    expect(formFieldIsRendered("Venue")).toBeTruthy();
}

export async function should_allow_user_to_create_new_event() {
    renderEventsManagement();
    await clickAddEventIcon();
    expect(eventFormIsRendered()).toBeTruthy();

    const eventName = "Test Event";

    const startEventDate = new Date();
    startEventDate.setDate(startEventDate.getDate() + 1);
    const startEventDateStringWithTime = startEventDate.toISOString().split("T")[0] + "T12:12";

    const endEventDate = new Date(startEventDate);
    endEventDate.setDate(endEventDate.getDate() + 2);
    const endEventDateStringWithTime = endEventDate.toISOString().split("T")[0] + "T13:13";
    const eventVenue = Venue.O2ArenaLondon;

    await fillEventForm({
        eventName: eventName,
        startDate: startEventDateStringWithTime,
        endDate: endEventDateStringWithTime,
        venue: eventVenue
    });
    await clickSubmitEventButton();
    await waitUntil(wait_for_post);
    const data = mockServer.content
    expect(data).toEqual({
        EventName: eventName,
        StartDate: startEventDate.toISOString().split("T")[0] + "T12:12:00" + ".000Z",
        EndDate: endEventDate.toISOString().split("T")[0] + "T13:13:00" + ".000Z",
        Venue: eventVenue
    });
}

export async function should_navigate_back_to_events_list_when_back_button_is_clicked() {
    renderEventsManagement();
    await waitUntil(wait_for_get_events);
    await clickAddEventIcon();

    expect(eventFormIsRendered()).toBeTruthy();
    expect(backButtonIsRendered()).toBeTruthy();

    mockServer.reset();
    wait_for_get_events = mockServer.get("/events", Events);
    mockServer.start();

    await clickBackButton();

    expect(eventFormIsRendered()).toBeFalsy();
    await waitUntil(wait_for_get_events);

    for (const event of Events) {
        expect(eventExists(event.EventName)).toBeTruthy();
    }
}
