export interface User {
    Id: string;
    FullName: string;
    Email: string;
    UserType: UserType;
}

export enum UserType{
    Customer= "Customer",
    Administrator = "Administrator",
}