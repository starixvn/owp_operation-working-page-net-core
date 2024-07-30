import { fireEvent, render } from '@testing-library/react';
import CreateTicketHandler from './CreateTicketHandler';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('../../createTicketNew/Helper/HelperCreateTicket');

const mockDispatch = jest.fn();
const mockFunction = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    pagePolicy: {
      permission: {
        HasWrite: true,
        ActorId: 8
      }
    },
    ticketList: {
      ticketList: [],
      loading: false
    },
    signalREventTicket: {
      owpMatchId: 123456
    }
  })
);
const recipientData = {
  data: [
    {
      ticketTypeRecipientId: 1,
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    },
    {
      ticketTypeRecipientId: 2,
      recipientId: 2,
      recipientType: 1,
      recipientName: 'sa2',
      recipientNickname: 'sa_nick2'
    }
  ]
};
describe('TicketList under test', () => {
  const props = {
    handleOpenCreateTicket: mockFunction
  };

  it('should call dispatch when click', () => {
    const { container } = render(<CreateTicketHandler {...props} />);
    const button = container.querySelector('.icon-plus');

    fireEvent.click(button);

    expect(mockFunction).toBeCalledTimes(1);
  });
});
