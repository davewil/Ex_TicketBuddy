import {render, type RenderResult} from "@testing-library/react";
import {MemoryRouter, Route, Routes} from "react-router-dom";
import {EventsManagement} from "../EventsManagement.tsx";
import {userEvent} from "@testing-library/user-event";

let renderedComponent: RenderResult;

export function renderEventsManagement() {
    renderedComponent = render(
        <MemoryRouter initialEntries={['/events-management']}>
            <Routes>
                <Route path="/events-management/*" element={<EventsManagement />} />
            </Routes>
        </MemoryRouter>);
    return renderedComponent;
}

export function unmountEventsManagement() {
    renderedComponent.unmount();
}

export function eventExists(eventName: string): boolean {
    return elements.theEvent(eventName) !== null;
}

export function clickAddEventIcon() {
    const icon = elements.addEventIcon();
    return userEvent.click(icon);
}

export function eventFormIsRendered() {
    return elements.eventForm() !== null;
}

export function formFieldIsRendered(fieldName: string) {
    return elements.formField(fieldName) !== null;
}

export async function fillEventForm(eventData: {
    eventName: string,
    startDate: string,
    endDate: string,
    venue: string
}) {
    const eventNameField = elements.theFormField("Event Name");
    await userEvent.clear(eventNameField);
    const startDateField = elements.theFormField("Start Date");
    await userEvent.clear(startDateField);
    const endDateField = elements.theFormField("End Date");
    await userEvent.clear(endDateField);
    await userEvent.type(elements.theFormField("Event Name"), eventData.eventName);
    await userEvent.type(elements.theFormField("Start Date"), eventData.startDate);
    await userEvent.type(elements.theFormField("End Date"), eventData.endDate);
    await userEvent.type(elements.theFormField("Venue"), eventData.venue);
}

export async function clickSubmitEventButtonToAddEvent() {
    const createButton = elements.createEventButton();
    return userEvent.click(createButton);
}

export async function clickSubmitEventButtonToUpdateEvent() {
    const updateButton = elements.updateEventButton();
    return userEvent.click(updateButton);
}

export function backButtonIsRendered() {
    return elements.backButton() !== null;
}

export async function clickBackButton() {
    const button = elements.backButton();
    return userEvent.click(button);
}

export async function clickEditButtonForEvent(eventName: string) {
    const editButton = elements.editButtonForEvent(eventName);
    return userEvent.click(editButton);
}

export function editButtonExistsForEvent(eventName: string): boolean {
    try {
        const editButton = elements.editButtonForEvent(eventName);
        return !!editButton;
    } catch (e) {
        return false;
    }
}

const elements = {
    theEvent: (eventName: string) => renderedComponent.getByText(eventName),
    addEventIcon: () => renderedComponent.getByRole("link", { name: /add event/i }),
    eventForm: () => renderedComponent.queryByTestId("event-creation-form"),
    formField: (name: string) => renderedComponent.queryByLabelText(name),
    theFormField: (name: string) => renderedComponent.getByLabelText(name),
    createEventButton: () => renderedComponent.getByRole("button", { name: /create event/i }),
    updateEventButton: () => renderedComponent.getByRole("button", { name: /update event/i }),
    backButton: () => renderedComponent.getByRole("button", { name: /back to events/i }),
    editButtonForEvent: (eventName: string) => {
        const eventElement = renderedComponent.getByText(eventName).closest('.event-item');
        if (!eventElement) {
            throw new Error(`Could not find event with name ${eventName}`);
        }
        return renderedComponent.getByTestId(`edit-event-${eventName}`);
    },
}
