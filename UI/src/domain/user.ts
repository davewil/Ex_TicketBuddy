export interface User {
    Id: string;
    FullName: string;
    Email: string;
    UserType: UserType;
}

export enum UserType{
    Customer = 0,
    Administrator = 1,
}