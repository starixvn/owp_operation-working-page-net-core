import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Dispatch, SetStateAction, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Recipient } from '../TypeNative';
import EditRecipient from './EditRecipient';
import useStore from '../../app/zustand/ticketTypeStore';

const setRecipients = jest.fn();
useStore.setState({
  recipients: [
    {
      recipientNickname: 'sa',
      recipientId: 8,
      recipientName: 'sa',
      recipientType: 1,
      subtypeName: 'Sub1',
      ticketTypeRecipientId: 2074
    }
  ],
  subtypeSelected: { subtypeId: 85, typeId: 1, subtypeName: 'Sub1' },
  setRecipients: setRecipients
});
jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn()
}));

const mockDispatch = jest.fn();
const mockSetRecipients = jest.fn() as Dispatch<SetStateAction<Recipient[]>>;

(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({
    policy: {
      permission: { Username: 'sa', UserId: 8, HasWrite: true, SiteId: 99 }
    }
  })
);
(useDispatch as jest.Mock).mockReturnValue(mockDispatch);

describe('EditRecipientPopup under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should match snapshot', () => {
    const { container } = render(<EditRecipient />);
    expect(container).toMatchSnapshot();
  });

  it('should handleDeleteRecipient  ', () => {
    const { container } = render(<EditRecipient />);
    const element = container.querySelector(`[class="icon-edit"]`);
    waitFor(() => {
      fireEvent.click(element!);
    });
    const icon = container.querySelector('.icon-edit');
    fireEvent.click(icon!);
    expect(icon).toBeInTheDocument();
  });
});
