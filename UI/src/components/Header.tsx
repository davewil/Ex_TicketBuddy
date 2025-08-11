import {HeaderBar, TicketStubImage, UserDetails, UserIcon, UserIconContainer, UsersDropdown} from "./Header.styles.tsx";
import {useUsersStore} from "../stores/users.store.ts";
import {useShallow} from "zustand/react/shallow";
import {useState} from "react";
import * as React from "react";

export const Header = () => {
    const { user, users } = useUsersStore(useShallow((state => ({
        user: state.user,
        users: state.users
    }))));

    const [showUserDetails, setShowUserDetails] = useState<boolean>(false);
    const onUserIconClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        setShowUserDetails(!showUserDetails);
    };

    return (
        <HeaderBar>
            <h1>TicketBuddy</h1>
            <TicketStubImage/>
            {user && <UserIconContainer onClick={onUserIconClick} data-testid="user-icon">
                <UserIcon />
            </UserIconContainer>}
            {user && showUserDetails && (
                <UserDetails>
                    <p>{user.FullName}</p>
                    <p>{user.Email}</p>
                </UserDetails>
            )}
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