import {render, type RenderResult, screen} from "@testing-library/react";
import {MemoryRouter} from "react-router-dom";
import {userEvent} from "@testing-library/user-event";
import {Main} from "../../App.tsx";

let renderedComponent: RenderResult;

export function renderEventsManagement() {
    renderedComponent = render(
        <MemoryRouter initialEntries={['/events-management']}>
            <Main/>
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

export function createEventFormIsRendered() {
    return elements.createEventForm() !== null;
}

export function updateEventFormIsRendered() {
    return elements.updateEventForm() !== null;
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
    const editButton = elements.editButtonForEvent(eventName);
    return !!editButton;
}

export function errorToastIsDisplayed(errorMessage: string): boolean {
    const toastElement = screen.getByText(errorMessage);
    return !!toastElement;
}

export function ticketPriceInputIsRendered(): boolean {
    return elements.ticketPriceInput() !== null;
}

export function releaseTicketsButtonIsRendered(): boolean {
    return elements.releaseTicketsButton() !== null;
}

export async function enterTicketPrice(price: string) {
    const priceInput = elements.ticketPriceInput()!;
    await userEvent.clear(priceInput);
    return userEvent.type(priceInput, price);
}

export async function clickReleaseTicketsButton() {
    const releaseButton = elements.releaseTicketsButton()!;
    return userEvent.click(releaseButton);
}

const elements = {
    theEvent: (eventName: string) => renderedComponent.getByText(eventName),
    addEventIcon: () => renderedComponent.getByRole("link", { name: /add event/i }),
    createEventForm: () => renderedComponent.queryByTestId("event-creation-form"),
    updateEventForm: () => renderedComponent.queryByTestId("event-update-form"),
    formField: (name: string) => renderedComponent.queryByLabelText(name),
    theFormField: (name: string) => renderedComponent.getByLabelText(name),
    createEventButton: () => renderedComponent.getByRole("button", { name: /create event/i }),
    updateEventButton: () => renderedComponent.getByRole("button", { name: /update event/i }),
    backButton: () => renderedComponent.getByRole("button", { name: /back to events/i }),
    ticketPriceInput: () => renderedComponent.queryByTestId("ticket-price-input"),
    releaseTicketsButton: () => renderedComponent.queryByRole("button", { name: /release tickets/i }),
    editButtonForEvent: (eventName: string) => {
        return renderedComponent.getByTestId(`edit-event-${eventName}`);
    },
    releaseTicketsButtonForEvent: (eventName: string) => {
        return renderedComponent.getByTestId(`release-tickets-${eventName}`);
    },
    ticketPriceInputForEvent: (eventName: string) => {
        return renderedComponent.getByTestId(`ticket-price-${eventName}`);
    }
}
