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

export function renderHome() {
    renderedComponent = render(
            <MemoryRouter>
                <App/>
            </MemoryRouter>)
    return renderedComponent;
}

export function homePageIsRendered() {
    return elements.home() !== null;
}

const elements = {
    home: () => renderedComponent.queryByText("I am the mocked Home component")
}