import {vi} from 'vitest';
import {render, type RenderResult} from "@testing-library/react";
import App from "../App.tsx";
import {MemoryRouter} from "react-router-dom";
import {userEvent} from "@testing-library/user-event";

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

export function userIconIsRendered() {
    return elements.userIcon() !== null;
}

export async function clickUserIcon() {
    const theUserIcon = await elements.theUserIcon();
    return userEvent.click(theUserIcon);
}

export function userEmailIsRendered(email: string) {
    return elements.userEmail(email);
}

const elements = {
    home: () => renderedComponent.queryByText("I am the mocked Home component"),
    usersDropdown: () => renderedComponent.queryByTestId("users-dropdown"),
    userIcon: () => renderedComponent.queryByTestId("user-icon"),
    theUserIcon: () => renderedComponent.findByTestId("user-icon"),
    userEmail: (email: string) => renderedComponent.findByText(email),
}