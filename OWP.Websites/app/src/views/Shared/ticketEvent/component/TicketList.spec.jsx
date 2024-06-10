import { fireEvent, render, screen } from '@testing-library/react';
import TicketList from './TicketList';
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
jest.mock('../../../EventAssignment/app/store', () => ({
  getState: jest.fn()
}));
const mockDispatch = jest.fn();

useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    ticketList: {
      ticketList: [
        {
          IsRead: true,
          LatestCreatedDate: '2022-10-21T14:31:09.353',
          LatestMessage: 'gjh',
          LatestUsername: 'sa_nickname',
          SubtypeName: null,
          TicketId: 3120
        }
      ],
      loading: false
    },
    signalREventTicket: {
      owpMatchId: 123456
    },
    pagePolicy: {
      permission: {
        HasWrite: true,
        ActorId: 8
      }
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
    onCloseTicketList: jest.fn(),
    updateDataSources: jest.fn(),
    dataSources: [
      {
        OWPMatchId: 123456,
        IsRead: true,
        HasTicket: true
      },
      { OWPMatchId: 1251214, IsRead: true, HasTicket: true }
    ],
    index: 0
  };
  it('render correct', () => {
    const { container } = render(<TicketList {...props} />);
    expect(container.querySelector('.ticket-list-popup')).toBeInTheDocument();
  });
});
