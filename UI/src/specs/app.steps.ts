import {homePageIsRendered, renderHome} from "./app.page.tsx";
import {expect} from "vitest";

export function should_default_to_home_page() {
    renderHome();
    expect(homePageIsRendered()).toBeTruthy();
}