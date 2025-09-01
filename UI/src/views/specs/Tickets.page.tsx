import {render, type RenderResult} from "@testing-library/react";
import {MemoryRouter} from "react-router-dom";
import {vi} from "vitest";
import {userEvent} from "@testing-library/user-event";
import {AppRoutes} from "../../App.tsx";

vi.mock("../Home", () => {
    return {
        Home: () => {
            return (
                <div>I am the mocked Home component</div>
            );
        }
    }
})

let renderedComponent: RenderResult;

export function renderTickets(eventId: string) {
    renderedComponent = render(
        <MemoryRouter initialEntries={[`/tickets/${eventId}`]}>
            <AppRoutes/>
        </MemoryRouter>);
    return renderedComponent;
}

export function unmountTickets() {
    renderedComponent?.unmount();
}

export function titleIsRendered(eventName: string) {
    return elements.title(eventName) !== null;
}

export function getSeatElement(seatNumber: number) {
    return elements.getSeatElement(seatNumber);
}

export function getSeatRow(rowNumber: number) {
    return elements.getSeatRow(rowNumber);
}

export function clickBackToEventsButton() {
    return userEvent.click(elements.backToEventsButton()!);
}

export function homePageIsRendered() {
    return elements.homePageIsRendered() !== null;
}

export const elements = {
    title: (eventName: string) => renderedComponent.getByRole('heading', {name: `Tickets for Event: ${eventName}`}),
    getSeatElement: (seatNumber: number)=> renderedComponent.container.querySelector(`[data-seat="${seatNumber}"]`),
    getSeatRow: (rowNumber: number)=> renderedComponent.container.querySelector(`[data-row="${rowNumber}"]`),
    backToEventsButton: () => renderedComponent.getByText('Back to Events'),
    homePageIsRendered: () => renderedComponent.getByText('I am the mocked Home component'),
};
