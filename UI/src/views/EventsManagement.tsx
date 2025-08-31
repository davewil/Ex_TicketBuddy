import React, {useEffect, useState} from 'react';
import {
    AddIcon,
    BackIcon,
    TicketIcon,
    EventItem,
    EventList,
    EventContent,
    EventActions,
    TicketReleaseContainer,
    TicketPriceInput,
    FormContainer,
    FormGroup,
    Input,
    Label,
    Select, TicketPriceContainer
} from './EventsManagement.styles.tsx';
import {ConvertVenueToString, type Event, Venue} from '../domain/event.ts';
import {getEventById, getEvents, postEvent, putEvent, releaseTickets} from "../api/events.api.ts";
import moment from 'moment'
import {Link, Outlet, Route, Routes, useNavigate, useParams} from "react-router-dom";
import {Button} from "../components/Button.styles.tsx";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

type EventFormData = {
    eventName: string;
    startDateTime: string;
    endDateTime: string;
    venue: Venue;
};

const initialFormData: EventFormData = {
    eventName: '',
    startDateTime: '',
    endDateTime: '',
    venue: Venue.O2ArenaLondon,
};

export const EventsManagement = () => {
    return (
        <>
            <Routes>
                <Route index element={<ListEvents />} />
                <Route path="add" element={<AddEvent />} />
                <Route path="edit/:id" element={<EditEvent />} />
            </Routes>
            <Outlet />
        </>
    );
}

export const ListEvents = () => {
    const [events, setEvents] = useState<Event[]>([]);

    useEffect(() => {
        getEvents().then(data => {
            setEvents(data);
        });
    },[]);

    return (
        <>
            <h1>Events Management</h1>
            <Link to="add">
                <Button>
                    Add Event <AddIcon/>
                </Button>
            </Link>
            <EventList>
                {events.map((event, index) => (
                    <EventItem key={index} className="event-item">
                        <EventContent>
                            <h2>{event.EventName}</h2>
                            <p>{moment(event.StartDate).format('MMMM Do YYYY, h:mm A')} to {moment(event.EndDate).format('MMMM Do YYYY, h:mm A')}</p>
                            <p>Venue: {ConvertVenueToString(event.Venue)}</p>
                        </EventContent>
                        <EventActions>
                            <Link to={`edit/${event.Id}`}>
                                <Button data-testid={`edit-event-${event.EventName}`}>
                                    Edit Event
                                </Button>
                            </Link>
                        </EventActions>
                    </EventItem>
                ))}
            </EventList>
        </>
    );
}

export const AddEvent = () => {
    const [formData, setFormData] = useState<EventFormData>(initialFormData);
    const navigate = useNavigate();

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (isFormValid()) {
            postEvent({
                EventName: formData.eventName,
                StartDate: moment(formData.startDateTime),
                EndDate: moment(formData.endDateTime),
                Venue: formData.venue,
            }).then(() => {
                setFormData(initialFormData);
                navigate('/events-management');
            }).catch((error) => {
                if (error.error && Array.isArray(error.error)) {
                    error.error.forEach((errorMessage: string) => {
                        toast.error(errorMessage);
                    });
                } else {
                    toast.error("Failed to create event");
                }
            });
        }
    };

    const isFormValid = () => {
        return formData.eventName && formData.startDateTime && formData.endDateTime && formData.venue;
    };

    return (
        <>
            <h1>Events Management</h1>
            <Link to="/events-management">
                <Button data-testid="back-button">
                    <BackIcon /> Back to Events
                </Button>
            </Link>
            <FormContainer data-testid="event-creation-form" onSubmit={handleSubmit}>
                <h2>Create New Event</h2>

                <FormGroup>
                    <Label htmlFor="eventName">Event Name</Label>
                    <Input
                        type="text"
                        id="eventName"
                        name="eventName"
                        value={formData.eventName}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="startDateTime">Start Date</Label>
                    <Input
                        type="datetime-local"
                        id="startDateTime"
                        name="startDateTime"
                        value={formData.startDateTime}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="endDateTime">End Date</Label>
                    <Input
                        type="datetime-local"
                        id="endDateTime"
                        name="endDateTime"
                        value={formData.endDateTime}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="venue">Venue</Label>
                    <Select
                        id="venue"
                        name="venue"
                        value={formData.venue}
                        onChange={(e) => setFormData({ ...formData, venue: e.target.value as Venue })}
                    >
                        {Object.values(Venue).map((venue) => (
                            <option key={venue} value={venue}>
                                {ConvertVenueToString(venue)}
                            </option>
                        ))}
                    </Select>
                </FormGroup>

                <Button type="submit" disabled={!isFormValid()}>Create Event</Button>
            </FormContainer>
        </>
    );
};

export const EditEvent = () => {
    const [formData, setFormData] = useState<EventFormData>(initialFormData);
    const [ticketPrice, setTicketPrice] = useState<string>("");
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>() as { id: string };

    useEffect(() => {
        const fetchEventDetails = async () => {
            const event = await getEventById(id);
            setFormData({
                eventName: event.EventName,
                startDateTime: moment(event.StartDate).format('YYYY-MM-DDTHH:mm'),
                endDateTime: moment(event.EndDate).format('YYYY-MM-DDTHH:mm'),
                venue: event.Venue,
            });
        };

        fetchEventDetails();
    }, [id]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleTicketPriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setTicketPrice(e.target.value);
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (isFormValid()) {
            putEvent(id, {
                EventName: formData.eventName,
                StartDate: moment(formData.startDateTime),
                EndDate: moment(formData.endDateTime),
                Venue: formData.venue,
            }).then(() => {
                setFormData(initialFormData);
                navigate('/events-management');
            }).catch((error) => {
                if (error.error && Array.isArray(error.error)) {
                    error.error.forEach((errorMessage: string) => {
                        toast.error(errorMessage);
                    });
                } else {
                    toast.error("Failed to update event");
                }
            });
        }
    };

    const handleReleaseTickets = async () => {
        const price = parseFloat(ticketPrice);

        releaseTickets(id, price).then(() => {
            setTicketPrice("");
        }).catch((error) => {
            if (error.error && Array.isArray(error.error)) {
                error.error.forEach((errorMessage: string) => {
                    toast.error(errorMessage);
                });
            } else {
                toast.error("Failed to release tickets");
            }
        });
    };

    const isFormValid = () => {
        return formData.eventName && formData.startDateTime && formData.endDateTime && formData.venue;
    };

    return (
        <>
            <h1>Events Management</h1>
            <Link to="/events-management">
                <Button data-testid="back-button">
                    <BackIcon /> Back to Events
                </Button>
            </Link>
            <FormContainer data-testid="event-update-form" onSubmit={handleSubmit}>
                <h2>Edit Event</h2>

                <FormGroup>
                    <Label htmlFor="eventName">Event Name</Label>
                    <Input
                        type="text"
                        id="eventName"
                        name="eventName"
                        value={formData.eventName}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="startDateTime">Start Date</Label>
                    <Input
                        type="datetime-local"
                        id="startDateTime"
                        name="startDateTime"
                        value={formData.startDateTime}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="endDateTime">End Date</Label>
                    <Input
                        type="datetime-local"
                        id="endDateTime"
                        name="endDateTime"
                        value={formData.endDateTime}
                        onChange={handleInputChange}
                    />
                </FormGroup>

                <FormGroup>
                    <Label htmlFor="venue">Venue</Label>
                    <Select
                        id="venue"
                        name="venue"
                        value={formData.venue}
                        onChange={(e) => setFormData({ ...formData, venue: e.target.value as Venue })}
                    >
                        {Object.values(Venue).map((venue) => (
                            <option key={venue} value={venue}>
                                {ConvertVenueToString(venue)}
                            </option>
                        ))}
                    </Select>
                </FormGroup>

                <Button type="submit" data-testid="update-event-button" disabled={!isFormValid()}>Update Event</Button>
            </FormContainer>

            <TicketReleaseContainer>
                <h3>Release Tickets</h3>
                <TicketPriceContainer>
                    <span>£</span>
                    <TicketPriceInput
                        type="number"
                        placeholder="Price"
                        step="0.01"
                        value={ticketPrice}
                        onChange={handleTicketPriceChange}
                        data-testid="ticket-price-input"
                    />
                    <Button
                        onClick={handleReleaseTickets}
                        data-testid="release-tickets-button"
                    >
                        <TicketIcon /> Release Tickets
                    </Button>
                </TicketPriceContainer>
            </TicketReleaseContainer>
        </>
    );
};
