import {MockServer} from "../../testing/mock-server.ts";
import {afterEach, beforeEach, expect} from "vitest";
import {
    renderTickets,
    getSeatElement,
    unmountTickets,
    getSeatRow,
    titleIsRendered,
    clickBackToEventsButton,
    homePageIsRendered
} from "./Tickets.page.tsx";
import {waitUntil} from "../../testing/utilities.ts";
import {Events, TicketsForFirstEvent} from "../../testing/data.ts";

const mockServer = MockServer.New();
let wait_for_get_event: () => boolean;
let wait_for_get_tickets: () => boolean;


beforeEach(() => {
    mockServer.reset();
    wait_for_get_event = mockServer.get(`events/${Events[0].Id}`, Events[0]);
    wait_for_get_tickets = mockServer.get(`events/${Events[0].Id}/tickets`, TicketsForFirstEvent);
    mockServer.start();
});

afterEach(() => {
    unmountTickets();
});

export async function should_display_seat_map_for_event() {
    renderTickets(Events[0].Id);
    await waitUntil(wait_for_get_event);
    await waitUntil(wait_for_get_tickets);

    for (let i = 1; i <= 10; i++) {
        const seat = getSeatElement(i);
        expect(seat).not.toBeNull();
    }

    expect(titleIsRendered(Events[0].EventName)).toBeTruthy();
    expect(getSeatRow(1)).not.toBeNull();
    expect(getSeatRow(2)).not.toBeNull();
}

export async function should_navigate_back_to_events() {
    renderTickets(Events[0].Id);
    await waitUntil(wait_for_get_event);
    await waitUntil(wait_for_get_tickets);
    await clickBackToEventsButton();
    expect(homePageIsRendered()).toBeTruthy();
}
