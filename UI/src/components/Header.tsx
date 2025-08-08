import {HeaderBar, TicketStubImage} from "./Header.styles.tsx";

export const Header = () => {
    return (
        <HeaderBar>
            <h1>TicketBuddy</h1>
            <TicketStubImage/>
        </HeaderBar>
    );
}