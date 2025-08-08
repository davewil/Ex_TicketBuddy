import {render, type RenderResult} from "@testing-library/react";
import {MemoryRouter} from "react-router-dom";
import App from "../../App.tsx";

let renderedComponent: RenderResult;

export function renderHome() {
    renderedComponent = render(
        <MemoryRouter>
            <App/>
        </MemoryRouter>)
    return renderedComponent;
}

export function unmountHome() {
    renderedComponent.unmount();
}

export function eventExists(eventName: string): boolean {
    return elements.theEvent(eventName) !== null;
}

const elements = {
    theEvent: (eventName: string) => renderedComponent.getByText(eventName)
}