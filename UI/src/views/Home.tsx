import {useEffect, useState} from "react";
import {getEvents} from "../api/events.api";
import {type Event} from "../domain/event";

export const Home = () => {
    const [events, setEvents] = useState<Event[]>([]);

    useEffect(() => {
        getEvents().then(data => {
            setEvents(data);
        });
    },[]);

    return (
        <div>
            <h1>Events</h1>
            <ul>
                {events.map((event, index) => (
                    <li key={index}>{event.EventName}</li>
                ))}
            </ul>
        </div>
    );
}