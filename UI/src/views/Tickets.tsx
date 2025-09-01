import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getEventById, getTicketsForEvent } from '../api/events.api';
import { type Ticket } from '../domain/ticket';
import { type Event } from '../domain/event';
import {
  TicketsContainer,
  SeatMapContainer,
  SeatRow,
  Seat,
  ScreenArea,
  PriceInfo,
  Legend,
  LegendItem,
  LegendColor,
  EventTitle
} from './Tickets.styles';
import { Button } from '../components/Button.styles';
import { BackIcon } from './EventsManagement.styles';

const SEATS_PER_ROW = 5;

export const Tickets = () => {
  const { eventId } = useParams<{ eventId: string }>();
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [event, setEvent] = useState<Event | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchEventAndTickets = async () => {
      if (!eventId) return;
        await Promise.all([
            getEventById(eventId),
            getTicketsForEvent(eventId)
        ]).then(data => {
            setEvent(data[0]);
            setTickets(data[1]);
            setLoading(false);
        }).catch(_ => {
            setLoading(false);
        })
    };

    fetchEventAndTickets();
  }, [eventId]);

  const renderSeatMap = () => {
    const maxSeatNumber = Math.max(...tickets.map(ticket => ticket.SeatNumber), 0);
    const numRows = Math.ceil(maxSeatNumber / SEATS_PER_ROW);

    const rows = [];
    for (let rowIndex = 0; rowIndex < numRows; rowIndex++) {
      const startSeat = rowIndex * SEATS_PER_ROW + 1;
      const endSeat = Math.min((rowIndex + 1) * SEATS_PER_ROW, maxSeatNumber);

      const seats = [];
      for (let seatNumber = startSeat; seatNumber <= endSeat; seatNumber++) {
        const ticket = tickets.find(t => t.SeatNumber === seatNumber);
        const isBooked = ticket ? !!ticket.UserId : false;

        seats.push(
          <Seat
            key={seatNumber}
            isbooked={isBooked}
            className={isBooked ? 'booked' : ''}
            data-seat={seatNumber}
          >
            {seatNumber}
          </Seat>
        );
      }

      rows.push(
        <SeatRow key={rowIndex} data-row={rowIndex + 1}>
          {seats}
        </SeatRow>
      );
    }

    return rows;
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <TicketsContainer>
      <Link to="/">
        <Button data-testid="back-button">
          <BackIcon /> Back to Events
        </Button>
      </Link>

      <EventTitle>Tickets for Event: {event?.EventName}</EventTitle>

      <Legend>
        <LegendItem>
          <LegendColor color="#4CAF50" />
          <span>Available</span>
        </LegendItem>
        <LegendItem>
          <LegendColor color="#f5f5f5" />
          <span>Booked</span>
        </LegendItem>
      </Legend>

      <SeatMapContainer>
        <ScreenArea>STAGE</ScreenArea>
        {renderSeatMap()}
      </SeatMapContainer>

      {tickets.length > 0 && (
        <PriceInfo>
          Ticket Price: £{tickets[0].Price.toFixed(2)}
        </PriceInfo>
      )}
    </TicketsContainer>
  );
};
