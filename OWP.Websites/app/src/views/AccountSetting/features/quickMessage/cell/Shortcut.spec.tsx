import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { useAppDispatch } from '../../../app/hooks';
import { setEditModel } from '../slice/quickMessageSlice';
import Shortcut from './Shortcut';

jest.mock('../../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
}));

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;

beforeEach(() => {
  jest.clearAllMocks();
  mockUseAppDispatch.mockReturnValue(mockDispatch);
});

describe('Shortcut Component', () => {
  test('renders with provided shortcut value in display mode', () => {
    render(<Shortcut shortcut="/abc" isEdit={false} />);

    expect(screen.getByText('abc')).toBeInTheDocument();
  });

  test('renders input field with correct value in edit mode', () => {
    render(<Shortcut shortcut="/A" isEdit={true} />);

    const input = screen.getByRole('textbox') as HTMLInputElement;
    expect(input).toBeInTheDocument();
    expect(input.value).toBe('A'); // Expects 'A' because the slash is removed
  });

  test('dispatches setEditModel with trimmed value on blur', () => {
    render(<Shortcut shortcut="/A" isEdit={true} />);

    const input = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(input, { target: { value: ' NewValue ' } });
    fireEvent.blur(input);

    expect(mockDispatch).toHaveBeenCalledWith({
      type: setEditModel.type,
      payload: { property: 'shortcut', value: '/NewValue' }
    });
  });

  test('does not allow "/" character input', () => {
    render(<Shortcut shortcut="/A" isEdit={true} />);

    const input = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.keyDown(input, { key: '/', code: 'Slash' });

    // Ensuring "/" is not added to the input field
    expect(input.value).toBe('A'); // Only 'A' should be present
  });

  test('shows input as empty when shortcut is empty', () => {
    render(<Shortcut shortcut="" isEdit={true} />);

    const input = screen.getByRole('textbox') as HTMLInputElement;
    expect(input).toBeInTheDocument();
    expect(input.value).toBe(''); // No value should be present
    expect(input).toHaveClass('input-empty');
  });
});
