import styled from "styled-components";

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