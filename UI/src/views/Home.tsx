import {useEffect, useState} from "react";
import {getEvents} from "../api/events.api";
import {ConvertVenueToString, type Event} from "../domain/event";
import {EventItem, EventList} from "./Home.styles.tsx";
import moment from "moment";
import {Button} from "../components/Button.styles.tsx";
import {useNavigate} from "react-router-dom";

export const Home = () => {
    const [events, setEvents] = useState<Event[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        getEvents().then(data => {
            setEvents(data);
        });
    },[]);

    const handleFindTickets = (eventId: string) => {
        navigate(`/tickets/${eventId}`);
    };

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
                            <Button onClick={() => handleFindTickets(event.Id)}>Find Tickets</Button>
                        </div>
                    </EventItem>
                ))}
            </EventList>
        </div>
    );
}