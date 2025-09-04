import styled from "styled-components";

export const MainContainer = styled.div`
    padding: 32px;
    flex: 1;
    overflow-y: auto;
    overflow-x: hidden;
    margin-top: 96px;
    position: relative;
    
    &::before {
        content: '';
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-image: radial-gradient(circle at 20% 80%, rgba(14, 165, 233, 0.05) 0%, transparent 50%),
                          radial-gradient(circle at 80% 20%, rgba(14, 165, 233, 0.05) 0%, transparent 50%);
        pointer-events: none;
        z-index: -1;
    }
`