import styled from "styled-components";

export const EventList = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 20px;
`;

export const EventItem = styled.div`
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    width: 100%;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s;
    display: flex;
    align-items: flex-end;
    justify-content: flex-start;
    position: relative;
`;