import styled from "styled-components";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus} from "@fortawesome/free-solid-svg-icons";

export const Container = styled.div`
    padding: 20px;
`;

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

export const FormContainer = styled.form`
    margin: 10px 0;
    padding: 5px 0;
`;

export const FormGroup = styled.div`
    margin-bottom: 15px;
`;

export const Label = styled.label`
    display: block;
    margin-bottom: 5px;
    font-weight: bold;
`;

export const Input = styled.input`
    width: 100%;
    box-sizing: border-box;
    padding: 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 16px;
`;

export const Select = styled.select`
    width: 100%;
    padding: 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 16px;
`;

export const Button = styled.button`
    padding: 10px 15px;
    background-color: #61dafb;
    color: #282c34;
    border: none;
    border-radius: 4px;
    font-size: 16px;
    cursor: pointer;
    
    &:hover {
        background-color: #21a1f1;
    }
    
    &:disabled {
        background-color: #cccccc;
        cursor: not-allowed;
    }
`;

export const AddIcon = () => <FontAwesomeIcon icon={faPlus} />;
