import React, {useState} from 'react';
import {Button, Container, FormContainer, FormGroup, Input, Label, Select} from './EventsManagement.styles.tsx';
import {ConvertVenueToString, Venue} from '../domain/event.ts';
import {postEvent} from "../api/events.api.ts";

type EventFormData = {
    eventName: string;
    date: string;
    venue: Venue;
};

const initialFormData: EventFormData = {
    eventName: '',
    date: '',
    venue: Venue.O2ArenaLondon,
};

export const EventsManagement = () => {
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
                Date: new Date(formData.date),
                Venue: formData.venue,
            }).then(() => {
                setFormData(initialFormData);
            });
        }
    };

    const isFormValid = () => {
        return formData.eventName && formData.date && formData.venue;
    };

    return (
        <Container>
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
                    <Label htmlFor="date">Date</Label>
                    <Input
                        type="date"
                        id="date"
                        name="date"
                        value={formData.date}
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

                <Button
                    type="submit"
                    disabled={!isFormValid()}
                >
                    Create Event
                </Button>
            </FormContainer>
        </Container>
    );
};
