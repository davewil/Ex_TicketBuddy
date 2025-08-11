import {HeaderBar, TicketStubImage, UserIcon, UsersDropdown} from "./Header.styles.tsx";
import {useUsersStore} from "../stores/users.store.ts";
import {useShallow} from "zustand/react/shallow";

export const Header = () => {
    const { user, users } = useUsersStore(useShallow((state => ({
        user: state.user,
        users: state.users
    }))));

    return (
        <HeaderBar>
            <h1>TicketBuddy</h1>
            <TicketStubImage/>
            {user && <UserIcon/>}
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