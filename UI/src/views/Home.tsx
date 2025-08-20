import {useEffect, useState} from "react";
import {getEvents} from "../api/events.api";
import {ConvertVenueToString, type Event} from "../domain/event";
import {EventItem, EventList} from "./Home.styles.tsx";
import moment from "moment";

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
                            <p>{moment(event.StartDate).format('MMMM Do YYYY, h:mm A')} to {moment(event.EndDate).format('MMMM Do YYYY, h:mm A')}</p>
                            <p>Venue: {ConvertVenueToString(event.Venue)}</p>
                            <button>Find tickets</button>
                        </div>

                    </EventItem>
                ))}
            </EventList>
        </div>
    );
}