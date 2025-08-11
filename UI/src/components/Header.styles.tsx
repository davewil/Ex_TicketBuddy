import styled from "styled-components";
import TicketStub from "../assets/ticket-stub.svg";

export const HeaderBar = styled.div`
    display: flex;
    align-items: center;
    padding: 10px 20px;
    background-color: #282c34;
    color: white;
    font-size: 1.5em;
    
    h1 {
        margin: 0;
        font-size: 1.8em;
        font-weight: bold;
        color: #61dafb;
    }
`

export const TicketStubImage = () => <img src={TicketStub} alt="Ticket Stub" width={150} height={100} />;

export const UsersDropdown = styled.select`
    margin-left: auto;
    padding: 5px;
    font-size: 1em;
    border-radius: 5px;
    border: 1px solid #ccc;
    background-color: #fff;
    color: #333;
    cursor: pointer;
    &:focus {
        outline: none;
        border-color: #61dafb;
    }
    option {
        padding: 5px;
        background-color: #fff;
        color: #333;
    }
    option:hover {
        background-color: #f0f0f0;
    }
    option:checked {
        background-color: #61dafb;
        color: white;
    }
    option:disabled {
        color: #ccc;
    }
`