import {HeaderBar, TicketStubImage, UsersDropdown} from "./Header.styles.tsx";
import {useUsersStore} from "../stores/users.store.ts";
import {useShallow} from "zustand/react/shallow";

export const Header = () => {
    const { users } = useUsersStore(useShallow((state => ({
        users: state.users
    }))));

    return (
        <HeaderBar>
            <h1>TicketBuddy</h1>
            <TicketStubImage/>
            <UsersDropdown data-testid="users-dropdown">
                {users.map(user => (
                    <option key={user.Id} value={user.Id}>
                        {user.FullName}
                    </option>
                ))}
            </UsersDropdown>
        </HeaderBar>
    );
}