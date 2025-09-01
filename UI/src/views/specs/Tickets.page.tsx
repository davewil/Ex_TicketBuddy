import {render, type RenderResult} from "@testing-library/react";
import {MemoryRouter, Routes, Route} from "react-router-dom";
import {Tickets} from "../Tickets.tsx";

let renderedComponent: RenderResult;

export function renderTickets(eventId: string) {
    renderedComponent = render(
        <MemoryRouter initialEntries={[`/tickets/${eventId}`]}>
            <Routes>
                <Route path="/tickets/:eventId" element={<Tickets />} />
            </Routes>
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

export const elements = {
    title: (eventName: string) => renderedComponent.getByRole('heading', {name: `Tickets for Event: ${eventName}`}),
    getSeatElement: (seatNumber: number)=> renderedComponent.container.querySelector(`[data-seat="${seatNumber}"]`),
    getSeatRow: (rowNumber: number)=> renderedComponent.container.querySelector(`[data-row="${rowNumber}"]`),
};
