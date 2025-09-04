import styled, { css } from "styled-components";

type ButtonVariant = 'primary' | 'secondary' | 'ghost' | 'danger';
type ButtonSize = 'sm' | 'md' | 'lg';

interface ButtonProps {
  variant?: ButtonVariant;
  size?: ButtonSize;
  fullWidth?: boolean;
}

const variantStyles = {
  primary: css`
    background: var(--gradient-primary);
    color: white;
    border: none;
    box-shadow: var(--shadow-md);
    
    &:hover:not(:disabled) {
      box-shadow: var(--shadow-lg), var(--shadow-glow);
      transform: translateY(-2px);
    }
    
    &:active:not(:disabled) {
      transform: translateY(0);
      box-shadow: var(--shadow-sm);
    }
  `,

  secondary: css`
    background: rgba(255, 255, 255, 0.1);
    color: var(--gray-100);
    border: 1px solid var(--gray-600);
    backdrop-filter: blur(10px);
    
    &:hover:not(:disabled) {
      background: rgba(255, 255, 255, 0.15);
      border-color: var(--primary-500);
      box-shadow: var(--shadow-md);
      transform: translateY(-1px);
    }
  `,

  ghost: css`
    background: transparent;
    color: var(--primary-400);
    border: 1px solid transparent;
    
    &:hover:not(:disabled) {
      background: rgba(14, 165, 233, 0.1);
      border-color: var(--primary-500);
      color: var(--primary-300);
    }
  `,

  danger: css`
    background: linear-gradient(135deg, var(--error-500) 0%, #dc2626 100%);
    color: white;
    border: none;
    box-shadow: var(--shadow-md);
    
    &:hover:not(:disabled) {
      box-shadow: var(--shadow-lg), 0 0 20px rgba(239, 68, 68, 0.3);
      transform: translateY(-2px);
    }
  `
};

const sizeStyles = {
  sm: css`
    padding: 8px 16px;
    font-size: 14px;
    border-radius: 6px;
  `,

  md: css`
    padding: 12px 24px;
    font-size: 16px;
    border-radius: 8px;
  `,

  lg: css`
    padding: 16px 32px;
    font-size: 18px;
    border-radius: 10px;
  `
};

export const Button = styled.button<ButtonProps>`
  position: relative;
  font-weight: 600;
  cursor: pointer;
  transition: all var(--transition-normal);
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  white-space: nowrap;
  user-select: none;
  
  ${({ size = 'md' }) => sizeStyles[size]}
  ${({ variant = 'primary' }) => variantStyles[variant]}
  ${({ fullWidth }) => fullWidth && css`
    width: 100%;
  `}
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
    transform: none !important;
    box-shadow: none !important;
  }
  
  &:focus-visible {
    outline: 2px solid var(--primary-500);
    outline-offset: 2px;
  }
    
  &.loading {
    pointer-events: none;
    
    &::after {
      content: '';
      position: absolute;
      width: 16px;
      height: 16px;
      border: 2px solid transparent;
      border-top: 2px solid currentColor;
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }
  }
  
  @keyframes spin {
    to {
      transform: rotate(360deg);
    }
  }
`;