import { render, fireEvent, screen, act } from '@testing-library/react';
import Recipient from './Recipient';
import { TRecipient } from '../Type';

describe('Recipient', () => {
  it('renders the Recipient component correctly', () => {
    const selectedRecipients: TRecipient[] = [
      {
        groupName: null,
        userName: 'Recipient1',
        nickName: 'Recipient1',
        recipientId: 1,
        recipientType: 1
      },
      {
        groupName: null,
        userName: 'Recipient2',
        nickName: 'Recipient2',
        recipientId: 2,
        recipientType: 1
      }
    ];
    const handleOnChange = jest.fn();
    const handleOnErrorSubmit = jest.fn();
    const isErrorRecipient = false;

    render(
      <Recipient
        selectedRecipients={selectedRecipients}
        handleOnChange={handleOnChange}
        handleOnErrorSubmit={handleOnErrorSubmit}
        isErrorRecipient={isErrorRecipient}
      />
    );
    act(() => {
      const labelElement = screen.getByText('Recipient1');
      expect(labelElement).toBeInTheDocument();
    });
  });

  it('calls handleOnErrorSubmit when the errorSubmit value changes', () => {
    const selectedRecipients: TRecipient[] = [
      {
        groupName: null,
        userName: 'Recipient1',
        nickName: 'Recipient1',
        recipientId: 1,
        recipientType: 1
      },
      {
        groupName: null,
        userName: 'Recipient2',
        nickName: 'Recipient2',
        recipientId: 2,
        recipientType: 1
      }
    ];
    const handleOnChange = jest.fn();
    const handleOnErrorSubmit = jest.fn();
    const isErrorRecipient = false;

    const { container } = render(
      <Recipient
        selectedRecipients={selectedRecipients}
        handleOnChange={handleOnChange}
        handleOnErrorSubmit={handleOnErrorSubmit}
        isErrorRecipient={isErrorRecipient}
      />
    );

    const input = container.querySelector('.MuiInputBase-input');
    input && fireEvent.change(input, { target: { value: 's' } });
    expect(handleOnErrorSubmit).toBeCalledTimes(1);
  });
});
