import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {
    eventFormIsRendered,
    formFieldIsRendered,
    renderEventsManagement,
    unmountEventsManagement,
    fillEventForm,
    clickSubmitEventButton,
    formFieldIsReset, clickAddEventIcon
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
        StartDate: startEventDate.toISOString().split("T")[0] + "T11:12:00" + ".000Z",
        EndDate: endEventDate.toISOString().split("T")[0] + "T12:13:00" + ".000Z",
        Venue: eventVenue
    });
    expect(formFieldIsReset("Event Name")).toBeTruthy();
    expect(formFieldIsReset("Start Date")).toBeTruthy();
    expect(formFieldIsReset("End Date")).toBeTruthy();
}