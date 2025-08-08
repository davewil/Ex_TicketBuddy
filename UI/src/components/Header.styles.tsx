import styled from "styled-components";
import TicketStub from "../assets/ticket-stub.svg";

export const HeaderBar = styled.div`
    display: flex;
    align-items: center;
    padding: 10px 20px;
    background-color: #282c34;
    color: white;
    font-size: 1.5em;
    width: 100%;
    
    h1 {
        margin: 0;
        font-size: 1.8em;
        font-weight: bold;
        color: #61dafb;
    }
`

export const TicketStubImage = () => <img src={TicketStub} alt="Ticket Stub" width={150} height={100} />;