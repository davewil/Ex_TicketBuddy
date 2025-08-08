import {type Event, Venue} from '../domain/event';

export const Events : Event[] = [
    {
        Id: "1",
        Name: "Concert at O2 Arena",
        Date: new Date(new Date().setDate(new Date().getDate() + 1)),
        Venue: Venue.EmiratesOldTraffordManchester
    },
    {
        Id: "2",
        Name: "Football Match at Wembley Stadium",
        Date: new Date(new Date().setDate(new Date().getDate() + 2)),
        Venue: Venue.WembleyStadiumLondon
    },
    {
        Id: "3",
        Name: "Basketball Game at Manchester Arena",
        Date: new Date(new Date().setDate(new Date().getDate() + 3)),
        Venue: Venue.ManchesterArena
    },
    {
        Id: "4",
        Name: "Concert at Utilita Arena Birmingham",
        Date: new Date(new Date().setDate(new Date().getDate() + 4)),
        Venue: Venue.UtilitaArenaBirmingham
    },
    {
        Id: "5",
        Name: "Theatre Show at SSE Hydro Glasgow",
        Date: new Date(new Date().setDate(new Date().getDate() + 5)),
        Venue: Venue.UtilitaArenaBirmingham
    }
]