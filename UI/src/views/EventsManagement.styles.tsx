import styled from "styled-components";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPlus, faArrowLeft} from "@fortawesome/free-solid-svg-icons";

export const EventContent = styled.div`
    flex: 1;
`;

export const EventActions = styled.div`
    display: flex;
    flex-direction: column;
    gap: 16px;
`;

export const FormContainer = styled.form`
    margin: 20px 0;
    padding: 32px;
    background: var(--gradient-card);
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 16px;
    box-shadow: var(--shadow-lg);
    backdrop-filter: blur(20px);
`;

export const FormGroup = styled.div`
    margin-bottom: 24px;
`;

export const Label = styled.label`
    display: block;
    margin-bottom: 8px;
    font-weight: 600;
    color: var(--gray-100);
    font-size: 14px;
    text-transform: uppercase;
    letter-spacing: 0.05em;
`;

export const Input = styled.input`
    width: 100%;
    box-sizing: border-box;
    padding: 12px 16px;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid var(--gray-600);
    border-radius: 8px;
    color: var(--gray-100);
    font-size: 16px;
    transition: all var(--transition-normal);
    backdrop-filter: blur(10px);

    &:focus {
        outline: none;
        border-color: var(--primary-500);
        box-shadow: 0 0 0 3px rgba(14, 165, 233, 0.1);
        background: rgba(255, 255, 255, 0.08);
    }

    &::placeholder {
        color: var(--gray-400);
    }
`;

export const Select = styled.select`
    width: 100%;
    padding: 12px 16px;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid var(--gray-600);
    border-radius: 8px;
    color: var(--gray-100);
    font-size: 16px;
    cursor: pointer;
    transition: all var(--transition-normal);
    backdrop-filter: blur(10px);

    &:focus {
        outline: none;
        border-color: var(--primary-500);
        box-shadow: 0 0 0 3px rgba(14, 165, 233, 0.1);
        background: rgba(255, 255, 255, 0.08);
    }
    
    &:hover {
        border-color: var(--primary-400);
        background: rgba(255, 255, 255, 0.08);
    }
    
    option {
        background: var(--gray-800);
        color: var(--gray-100);
        padding: 12px;
        border: none;
    }
    
    option:hover {
        background: var(--gray-700);
    }
    
    option:checked {
        background: var(--primary-600);
        color: white;
    }
`;

export const AddIcon = () => <FontAwesomeIcon icon={faPlus} />;

export const BackIcon = () => <FontAwesomeIcon icon={faArrowLeft} style={{ marginRight: '5px' }} />;