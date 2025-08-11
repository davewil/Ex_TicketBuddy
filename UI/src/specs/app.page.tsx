import {vi} from 'vitest';
import {render, type RenderResult} from "@testing-library/react";
import App from "../App.tsx";
import {MemoryRouter} from "react-router-dom";

let renderedComponent: RenderResult;

vi.mock("../views/Home", () => {
    return {
        Home: () => {
            return (
                <div>I am the mocked Home component</div>
            );
        }
    }
})

export function renderApp() {
    renderedComponent = render(
            <MemoryRouter>
                <App/>
            </MemoryRouter>)
    return renderedComponent;
}

export function unmountApp() {
    renderedComponent.unmount();
}

export function homePageIsRendered() {
    return elements.home() !== null;
}

export function usersDropdownIsRendered() {
    return elements.usersDropdown() !== null;
}

const elements = {
    home: () => renderedComponent.queryByText("I am the mocked Home component"),
    usersDropdown: () => renderedComponent.queryByTestId("users-dropdown"),
}