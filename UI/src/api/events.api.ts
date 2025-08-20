import {get, post} from "../common/http.ts";
import {type Event, type EventPayload} from "../domain/event";

export const getEvents = async () => {
  return get<Event[]>("/events");
};

export const postEvent = async (event: EventPayload) => {
    const eventWithMoments = {
        ...event,
        StartDate: event.StartDate.toISOString(),
        EndDate: event.EndDate.toISOString(),
    }
    return post("/events", eventWithMoments);
}