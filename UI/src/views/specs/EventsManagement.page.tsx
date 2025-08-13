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

export function eventFormIsRendered() {
    return elements.eventForm() !== null;
}

export function formFieldIsRendered(fieldName: string) {
    return elements.formField(fieldName) !== null;
}

export async function fillEventForm(eventData: {
    eventName: string,
    date: string,
    venue: string,
    capacity: string
}) {
    await userEvent.type(elements.theFormField("eventName"), eventData.eventName);
    await userEvent.type(elements.theFormField("date"), eventData.date);
    await userEvent.type(elements.theFormField("venue"), eventData.venue);
    await userEvent.type(elements.theFormField("capacity"), eventData.capacity);
}

export async function clickSubmitEventButton() {
    const button = elements.submitEventButton();
    return userEvent.click(button);
}

export function submitButtonIsDisabled() {
    const button = elements.submitEventButton();
    return button.hasAttribute("disabled");
}

const elements = {
    eventForm: () => renderedComponent.queryByTestId("event-creation-form"),
    formField: (name: string) => renderedComponent.queryByLabelText(name),
    theFormField: (name: string) => renderedComponent.getByLabelText(name),
    submitEventButton: () => renderedComponent.getByRole("button", { name: /create event/i }),
}
