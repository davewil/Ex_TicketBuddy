import {render, type RenderResult} from "@testing-library/react";
import {MemoryRouter} from "react-router-dom";
import {EventsManagement} from "../EventsManagement.tsx";
import {userEvent} from "@testing-library/user-event";

let renderedComponent: RenderResult;

export function renderEventsManagement() {
    renderedComponent = render(
        <MemoryRouter>
            <EventsManagement/>
        </MemoryRouter>);
    return renderedComponent;
}

export function unmountEventsManagement() {
    renderedComponent.unmount();
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

export function formFieldIsReset(fieldName: string) {
    const field = elements.formField(fieldName)! as HTMLInputElement;
    return field.value === "";
}

export async function fillEventForm(eventData: {
    eventName: string,
    startDate: string,
    endDate: string,
    venue: string
}) {
    await userEvent.type(elements.theFormField("Event Name"), eventData.eventName);
    await userEvent.type(elements.theFormField("Start Date"), eventData.startDate);
    await userEvent.type(elements.theFormField("End Date"), eventData.endDate);
    await userEvent.type(elements.theFormField("Venue"), eventData.venue);
}

export async function clickSubmitEventButton() {
    const button = elements.submitEventButton();
    return userEvent.click(button);
}

const elements = {
    addEventIcon: () => renderedComponent.getByRole("link", { name: /add event/i }),
    eventForm: () => renderedComponent.queryByTestId("event-creation-form"),
    formField: (name: string) => renderedComponent.queryByLabelText(name),
    theFormField: (name: string) => renderedComponent.getByLabelText(name),
    submitEventButton: () => renderedComponent.getByRole("button", { name: /create event/i }),
}
