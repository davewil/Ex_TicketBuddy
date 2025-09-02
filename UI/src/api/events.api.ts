import {get, post, put} from "../common/http.ts";
import {type Event, type EventPayload, type UpdateEventPayload} from "../domain/event";
import {type Ticket} from "../domain/ticket";

export const getEvents = async () => {
  return get<Event[]>("/events");
};

export const getEventById = async (id: string) => {
    return get<Event>(`/events/${id}`);
}

export const getTicketsForEvent = async (eventId: string) => {
    return get<Ticket[]>(`/events/${eventId}/tickets`);
}

export const postEvent = async (event: EventPayload) => {
    const eventWithMoments = {
        ...event,
        StartDate: event.StartDate.toISOString(),
        EndDate: event.EndDate.toISOString(),
    }
    return post("/events", eventWithMoments);
}

export const putEvent = async (id: string, event: UpdateEventPayload) => {
    const eventWithMoments = {
        ...event,
        StartDate: event.StartDate.toISOString(),
        EndDate: event.EndDate.toISOString(),
    }
    return put(`/events/${id}`, eventWithMoments);
}

export type PurchaseTicketsPayload = {
    UserId: string;
    TicketIds: string[];
};

export const purchaseTickets = async (eventId: string, payload: PurchaseTicketsPayload) => {
    return post(`/events/${eventId}/tickets/purchase`, payload);
}
