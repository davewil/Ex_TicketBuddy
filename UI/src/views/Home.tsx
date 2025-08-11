import {useEffect, useState} from "react";
import {getEvents} from "../api/events.api";
import {type Event} from "../domain/event";
import {EventItem, EventList} from "./Home.styles.tsx";

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
            <EventList>
                {events.map((event, index) => (
                    <EventItem key={index}>
                        <div>
                            <h2>{event.EventName}</h2>
                            <p>{new Date(event.Date).toLocaleDateString()}</p>
                            <button>Find tickets</button>
                        </div>

                    </EventItem>
                ))}
            </EventList>
        </div>
    );
}