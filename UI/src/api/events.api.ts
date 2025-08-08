import {get} from "../common/http.ts";
import {type Event} from "../domain/event";

export const getEvents = async () => {
  return get<Event[]>("/events");
};