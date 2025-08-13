import {get, post} from "../common/http.ts";
import {type Event, type EventPayload} from "../domain/event";

export const getEvents = async () => {
  return get<Event[]>("/events");
};

export const postEvent = async (event: EventPayload) => {
    return post("/events", event);
}