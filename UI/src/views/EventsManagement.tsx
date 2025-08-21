import React, {useEffect, useState} from 'react';
import {
    AddIcon,
    Button,
    EventItem, EventList,
    FormContainer,
    FormGroup,
    Input,
    Label,
    Select
} from './EventsManagement.styles.tsx';
import {ConvertVenueToString, type Event, Venue} from '../domain/event.ts';
import {getEvents, postEvent} from "../api/events.api.ts";
import moment from 'moment'
import {Link, Outlet, Route, Routes} from "react-router-dom";

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
            <Link to="add">Add Event <AddIcon/></Link>
            <EventList>
                {events.map((event, index) => (
                    <EventItem key={index}>
                        <div>
                            <h2>{event.EventName}</h2>
                            <p>{moment(event.StartDate).format('MMMM Do YYYY, h:mm A')} to {moment(event.EndDate).format('MMMM Do YYYY, h:mm A')}</p>
                            <p>Venue: {ConvertVenueToString(event.Venue)}</p>
                        </div>

                    </EventItem>
                ))}
            </EventList>
        </>
    );
}

export const AddEvent = () => {
    const [formData, setFormData] = useState<EventFormData>(initialFormData);

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
            });
        }
    };

    const isFormValid = () => {
        return formData.eventName && formData.startDateTime && formData.endDateTime && formData.venue;
    };

    return (
        <>
            <h1>Events Management</h1>
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
