import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { useAppDispatch } from '../../../app/hooks';
import { setEditModel } from '../slice/quickMessageSlice';
import Message from './Message';

jest.mock('../../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
}));

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;

beforeEach(() => {
  jest.clearAllMocks();
  mockUseAppDispatch.mockReturnValue(mockDispatch);
});

describe('Message Component', () => {
  test('renders with provided message value in display mode', () => {
    render(
      <table>
        <tbody>
          <tr>
            <Message message="Test Message" isEdit={false} />
          </tr>
        </tbody>
      </table>
    );

    expect(screen.getByText('Test Message')).toBeInTheDocument();
  });

  test('renders input field with correct value in edit mode', () => {
    render(
      <table>
        <tbody>
          <tr>
            <Message message="Test Message" isEdit={true} />
          </tr>
        </tbody>
      </table>
    );

    const input = screen.getByRole('textbox') as HTMLInputElement;
    expect(input).toBeInTheDocument();
    expect(input.value).toBe('Test Message');
  });

  test('dispatches setEditModel with new value on blur', () => {
    render(
      <table>
        <tbody>
          <tr>
            <Message message="Test Message" isEdit={true} />
          </tr>
        </tbody>
      </table>
    );

    const input = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(input, { target: { value: 'Updated Message' } });
    fireEvent.blur(input);

    expect(mockDispatch).toHaveBeenCalledWith({
      type: setEditModel.type,
      payload: { property: 'message', value: 'Updated Message' }
    });
  });

  test('shows input as empty when message is empty', () => {
    render(
      <table>
        <tbody>
          <tr>
            <Message message="" isEdit={true} />
          </tr>
        </tbody>
      </table>
    );

    const input = screen.getByRole('textbox') as HTMLInputElement;
    expect(input).toBeInTheDocument();
    expect(input.value).toBe('');
    expect(input).toHaveClass('input-empty');
  });

  test('does not apply input-empty class when message is not empty', () => {
    render(
      <table>
        <tbody>
          <tr>
            <Message message="Non-empty message" isEdit={true} />
          </tr>
        </tbody>
      </table>
    );

    const input = screen.getByRole('textbox') as HTMLInputElement;
    expect(input).toBeInTheDocument();
    expect(input).toHaveClass('message-input');
    expect(input).not.toHaveClass('input-empty');
  });
});
