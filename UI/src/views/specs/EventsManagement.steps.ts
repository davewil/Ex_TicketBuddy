import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {
    createEventFormIsRendered,
    formFieldIsRendered,
    renderEventsManagement,
    unmountEventsManagement,
    fillEventForm,
    clickSubmitEventButtonToAddEvent,
    clickAddEventIcon,
    eventExists,
    backButtonIsRendered,
    clickBackButton,
    clickEditButtonForEvent,
    editButtonExistsForEvent,
    clickSubmitEventButtonToUpdateEvent,
    errorToastIsDisplayed,
    ticketPriceInputIsRendered,
    clickReleaseTicketsButton,
    enterTicketPrice, releaseTicketsButtonIsRendered, updateEventFormIsRendered
} from "./EventsManagement.page.tsx";
import { Venue } from "../../domain/event.ts";
import {waitUntil} from "../../testing/utilities.ts";
import {Events} from "../../testing/data.ts";

const mockServer = MockServer.New();
let wait_for_post: () => boolean;
let wait_for_put: () => boolean;
let wait_for_get_events: () => boolean;
let wait_for_get_event: () => boolean;

beforeEach(() => {
    mockServer.reset();
    wait_for_get_events = mockServer.get("/events", Events);
    wait_for_get_event = mockServer.get(`/events/${Events[0].Id}`, Events[0]);
    wait_for_post = mockServer.post("/events", {}, true, undefined);
    wait_for_put = mockServer.put(`/events/${Events[0].Id}`, {}, true);
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
    expect(createEventFormIsRendered()).toBeTruthy();
    expect(formFieldIsRendered("Event Name")).toBeTruthy();
    expect(formFieldIsRendered("Start Date")).toBeTruthy();
    expect(formFieldIsRendered("End Date")).toBeTruthy();
    expect(formFieldIsRendered("Venue")).toBeTruthy();
}

export async function should_allow_user_to_create_new_event() {
    renderEventsManagement();
    await clickAddEventIcon();
    expect(createEventFormIsRendered()).toBeTruthy();

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
    await clickSubmitEventButtonToAddEvent();
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

    expect(createEventFormIsRendered()).toBeTruthy();
    expect(backButtonIsRendered()).toBeTruthy();

    mockServer.reset();
    wait_for_get_events = mockServer.get("/events", Events);
    mockServer.start();

    await clickBackButton();

    expect(createEventFormIsRendered()).toBeFalsy();
    await waitUntil(wait_for_get_events);

    for (const event of Events) {
        expect(eventExists(event.EventName)).toBeTruthy();
    }
}

export async function should_allow_user_to_edit_existing_event() {
    renderEventsManagement();
    await waitUntil(wait_for_get_events);

    const eventToEdit = Events[0];
    expect(editButtonExistsForEvent(eventToEdit.EventName)).toBeTruthy();

    await clickEditButtonForEvent(eventToEdit.EventName);
    await waitUntil(wait_for_get_event);

    const updatedEventName = `${eventToEdit.EventName} - Updated`;
    const startEventDate = new Date();
    startEventDate.setDate(startEventDate.getDate() + 7);
    const startEventDateStringWithTime = startEventDate.toISOString().split("T")[0] + "T14:00";

    const endEventDate = new Date(startEventDate);
    endEventDate.setDate(endEventDate.getDate() + 1);
    const endEventDateStringWithTime = endEventDate.toISOString().split("T")[0] + "T17:00";

    await fillEventForm({
        eventName: updatedEventName,
        startDate: startEventDateStringWithTime,
        endDate: endEventDateStringWithTime,
        venue: eventToEdit.Venue
    });

    await clickSubmitEventButtonToUpdateEvent();
    await waitUntil(wait_for_put);

    const data = mockServer.content;
    expect(data).toEqual({
        EventName: updatedEventName,
        StartDate: startEventDate.toISOString().split("T")[0] + "T14:00:00" + ".000Z",
        EndDate: endEventDate.toISOString().split("T")[0] + "T17:00:00" + ".000Z",
        Venue: eventToEdit.Venue
    });

    expect(updateEventFormIsRendered()).toBeFalsy();
}

export async function should_show_error_toast_when_event_update_fails() {
    renderEventsManagement();
    await waitUntil(wait_for_get_events);

    const eventToEdit = Events[0];
    expect(editButtonExistsForEvent(eventToEdit.EventName)).toBeTruthy();

    await clickEditButtonForEvent(eventToEdit.EventName);
    await waitUntil(wait_for_get_event);

    mockServer.reset();
    const errorResponse = {
        Message: "The request could not be correctly validated.",
        Errors: ["End date cannot be before start date"]
    };
    wait_for_put = mockServer.put(`/events/${eventToEdit.Id}`, errorResponse, false);
    mockServer.start();

    const currentDate = new Date();
    const startDate = new Date();
    startDate.setDate(currentDate.getDate() + 10);
    const endDate = new Date();
    endDate.setDate(currentDate.getDate() + 5);

    const startDateString = startDate.toISOString().split("T")[0] + "T14:00";
    const endDateString = endDate.toISOString().split("T")[0] + "T12:00";

    await fillEventForm({
        eventName: "Updated Event Name",
        startDate: startDateString,
        endDate: endDateString,
        venue: eventToEdit.Venue
    });

    await clickSubmitEventButtonToUpdateEvent();
    await waitUntil(wait_for_put);

    expect(errorToastIsDisplayed("End date cannot be before start date")).toBeTruthy();
}

export async function should_show_error_toast_when_event_creation_fails() {
    renderEventsManagement();
    await waitUntil(wait_for_get_events);

    await clickAddEventIcon();
    expect(createEventFormIsRendered()).toBeTruthy();

    mockServer.reset();
    const errorResponse = {
        Message: "The request could not be correctly validated.",
        Errors: ["End date cannot be before start date"]
    };
    wait_for_post = mockServer.post("/events", errorResponse, false, 400);
    mockServer.start();

    const currentDate = new Date();
    const startDate = new Date();
    startDate.setDate(currentDate.getDate() + 10);
    const endDate = new Date();
    endDate.setDate(currentDate.getDate() + 5);

    const startDateString = startDate.toISOString().split("T")[0] + "T14:00";
    const endDateString = endDate.toISOString().split("T")[0] + "T12:00";

    await fillEventForm({
        eventName: "Invalid Event",
        startDate: startDateString,
        endDate: endDateString,
        venue: Venue.O2ArenaLondon
    });

    await clickSubmitEventButtonToAddEvent();
    await waitUntil(wait_for_post);

    expect(errorToastIsDisplayed("End date cannot be before start date")).toBeTruthy();
}

export async function should_allow_user_to_release_tickets_for_event() {
    let wait_for_post_tickets: () => boolean;

    renderEventsManagement();
    await waitUntil(wait_for_get_events);

    const eventToReleaseTicketsFor = Events[0];

    await clickEditButtonForEvent(eventToReleaseTicketsFor.EventName);
    await waitUntil(wait_for_get_event);

    wait_for_post_tickets = mockServer.post(`/events/${eventToReleaseTicketsFor.Id}/tickets`, {}, true);

    expect(ticketPriceInputIsRendered()).toBeTruthy();
    expect(releaseTicketsButtonIsRendered()).toBeTruthy();

    await enterTicketPrice("25.99");
    await clickReleaseTicketsButton();

    await waitUntil(wait_for_post_tickets);

    const data = mockServer.content;
    expect(data).toEqual({
        Price: 25.99
    });

    expect(updateEventFormIsRendered()).toBeFalsy();
}

export async function should_show_error_toast_when_ticket_release_fails() {
    let wait_for_post_tickets_error: () => boolean;

    renderEventsManagement();
    await waitUntil(wait_for_get_events);

    const eventToReleaseTicketsFor = Events[0];

    await clickEditButtonForEvent(eventToReleaseTicketsFor.EventName);
    await waitUntil(wait_for_get_event);

    mockServer.reset();
    const errorResponse = {
        Message: "The request could not be correctly validated.",
        Errors: ["Ticket price must be greater than zero"]
    };
    wait_for_post_tickets_error = mockServer.post(`/events/${eventToReleaseTicketsFor.Id}/tickets`, errorResponse, false);
    mockServer.start();

    expect(ticketPriceInputIsRendered()).toBeTruthy();
    expect(releaseTicketsButtonIsRendered()).toBeTruthy();

    await enterTicketPrice("-5.00");
    await clickReleaseTicketsButton();

    await waitUntil(wait_for_post_tickets_error);

    expect(errorToastIsDisplayed("Ticket price must be greater than zero")).toBeTruthy();
}
