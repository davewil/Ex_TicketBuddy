import {type Event, Venue} from '../domain/event';
import {type User, UserType} from "../domain/user.ts";
import moment from "moment";

export const Events : Event[] = [
    {
        Id: "1",
        EventName: "Concert at O2 Arena",
        StartDate: moment(new Date().setDate(new Date().getDate() + 1)),
        EndDate: moment(new Date().setDate(new Date().getDate() + 1 + 2)),
        Venue: Venue.EmiratesOldTraffordManchester
    },
    {
        Id: "2",
        EventName: "Football Match at Wembley Stadium",
        StartDate: moment(new Date().setDate(new Date().getDate() + 2)),
        EndDate: moment(new Date().setDate(new Date().getDate() + 2 + 3)),
        Venue: Venue.WembleyStadiumLondon
    },
    {
        Id: "3",
        EventName: "Basketball Game at Manchester Arena",
        StartDate: moment(new Date().setDate(new Date().getDate() + 3)),
        EndDate: moment(new Date().setDate(new Date().getDate() + 3 + 1)),
        Venue: Venue.ManchesterArena
    },
    {
        Id: "4",
        EventName: "Concert at Utilita Arena Birmingham",
        StartDate: moment(new Date().setDate(new Date().getDate() + 4)),
        EndDate: moment(new Date().setDate(new Date().getDate() + 4 + 1)),
        Venue: Venue.UtilitaArenaBirmingham
    },
    {
        Id: "5",
        EventName: "Theatre Show at SSE Hydro Glasgow",
        StartDate: moment(new Date().setDate(new Date().getDate() + 5)),
        EndDate: moment(new Date().setDate(new Date().getDate() + 5 + 1)),
        Venue: Venue.UtilitaArenaBirmingham
    }
]

export const Users : User[] = [
    {
        Id: "1",
        FullName: "John Doe",
        Email: "john.doe@customer.co.uk",
        UserType: UserType.Customer
    },
    {
        Id: "2",
        FullName: "Jane Doe",
        Email: "jane.doe@customer.co.uk",
        UserType: UserType.Customer
    },
    {
        Id: "3",
        FullName: "Will Chan",
        Email: "will.chan@customers.co.uk",
        UserType: UserType.Customer
    },
    {
        Id: "4",
        FullName: "Sean Connery",
        Email: "sean.connery@ticketbuddy.co.uk",
        UserType: UserType.Administrator
    },
    {
        Id: "5",
        FullName: "Roger Moore",
        Email: "roger.moore@ticketbuddy.co.uk",
        UserType: UserType.Administrator
    },
]