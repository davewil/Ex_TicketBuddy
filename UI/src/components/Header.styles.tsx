import styled from "styled-components";
import TicketStub from "../assets/ticket-stub.svg";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { Link } from 'react-router-dom';

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

export const UserIconContainer = styled.div`
    margin-left: auto;
    font-size: 1.5em;
    cursor: pointer;
    &:hover {
        color: #21a1f1;
    }
`;

export const UserIcon = () => <FontAwesomeIcon icon={faUser} />;

export const UsersDropdown = styled.select`
    margin-left: 10px;
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

export const UserDetails = styled.div`
    position: absolute;
    top: 50px;
    right: 250px;
    background-color: white;
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    z-index: 1000;
    h2 {
        margin: 0;
        font-size: 1.2em;
        color: #333;
    }
    p {
        margin: 5px 0;
        font-size: 1em;
        color: #666;
    }
`;

export const EventsManagementLink = styled(Link)`
    margin-left: 20px;
    color: #61dafb;
    text-decoration: none;
    &:hover {
        text-decoration: underline;
    }
    &:visited {
        color: #61dafb;
    }
    &:active {
        color: #21a1f1;
    }
    &:focus {
        outline: none;
    }
`;