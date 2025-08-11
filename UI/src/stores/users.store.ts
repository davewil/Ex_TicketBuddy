import {create} from "zustand";
import type {User} from "../domain/user.ts";
import {getUsers} from "../api/users.api.ts";

async function GetUsers() {
    return await getUsers();
}

export type UsersStore = {
    user: User | null;
    users: User[];
    loading: boolean;
    fetchUsers: () => Promise<void>;
}

export const useUsersStore = create<UsersStore>((set) => ({
        user: null,
        users: [],
        loading: true,
        fetchUsers: async () => {
            set({ loading: true });
            const users = await GetUsers();
            if (users.length === 0) {
                set({ user: null, users: [], loading: false });
                return;
            }

            set({
                user: users[0],
                users: users,
                loading: false
            });
        },
    }
))