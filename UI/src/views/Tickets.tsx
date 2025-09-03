import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
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
    EventTitle,
    SelectionInfo, CenteredButtonContainer
} from './Tickets.styles';
import { Button } from '../components/Button.styles';
import { BackIcon } from './EventsManagement.styles';

const SEATS_PER_ROW = 5;

export const Tickets = () => {
  const { eventId } = useParams<{ eventId: string }>();
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [event, setEvent] = useState<Event | null>(null);
  const [loading, setLoading] = useState(true);
  const [selectedSeats, setSelectedSeats] = useState<number[]>([]);
  const navigate = useNavigate();

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

  const handleSeatClick = (seatNumber: number) => {
    const ticket = tickets.find(t => t.SeatNumber === seatNumber);
    if (ticket?.Purchased) {
      return;
    }

    setSelectedSeats(prevSelectedSeats => {
      if (prevSelectedSeats.includes(seatNumber)) {
        return prevSelectedSeats.filter(seat => seat !== seatNumber);
      } else {
        return [...prevSelectedSeats, seatNumber];
      }
    });
  };

  const proceedToPurchase = () => {
    if (selectedSeats.length > 0 && eventId) {
      navigate(`/tickets/${eventId}/purchase`, {
        state: {
          selectedTickets: tickets.filter(t => selectedSeats.includes(t.SeatNumber)),
          event: event
        }
      });
    }
  };

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
        const isBooked = ticket ? ticket.Purchased : false;
        const isSelected = selectedSeats.includes(seatNumber);

        seats.push(
          <Seat
            key={seatNumber}
            isbooked={isBooked}
            isselected={isSelected}
            className={isBooked ? 'booked' : isSelected ? 'selected' : ''}
            data-seat={seatNumber}
            onClick={() => handleSeatClick(seatNumber)}
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

  const calculateTotalPrice = () => {
    if (tickets.length === 0 || selectedSeats.length === 0) return 0;
    const ticketPrice = tickets[0].Price;
    return ticketPrice * selectedSeats.length;
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
        <LegendItem>
          <LegendColor color="#FF9800" />
          <span>Selected</span>
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

      {selectedSeats.length > 0 && (
        <SelectionInfo>
          You have selected {selectedSeats.length} seats.
          <br />
          Seats: {selectedSeats.sort((a, b) => a - b).join(', ')}
          <br />
          Total: £{calculateTotalPrice().toFixed(2)}
        </SelectionInfo>
      )}

        <CenteredButtonContainer>
            <Button
                onClick={proceedToPurchase}
                disabled={selectedSeats.length === 0}
            >
                Proceed to Purchase
            </Button>
        </CenteredButtonContainer>

    </TicketsContainer>
  );
};
