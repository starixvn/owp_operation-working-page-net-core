import TicketDetailInformation from './TicketDetailInformation';
import useStableDispatch from '../customHook/useStableDispatch';
import { render, screen } from '@testing-library/react';
import { useSelector } from 'react-redux';

jest.mock('../customHook/useStableDispatch');

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useCallback: jest.fn()
}));

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
const mockedDispatch = jest.fn();

useStableDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    chatBoard: {
      ticketDetail: {
        ticketSubTypeId: 1,
        ticketSubTypeName: 'test',
        ticketId: 123
      },
      ticketSubTypeOptions: []
    },
    policy: {
      permission: {
        Username: 'sa',
        ActorId: 8,
        HasWrite: false
      }
    }
  })
);
describe('Ticket Detail Information', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render', () => {
    const props = {
      policy: 'policy',
      ticketSubTypeOptions: [{ subTypeId: 61, subTypeName: 'test' }],
      permission: {
        HasWrite: false,
        ActorId: 8
      },
      ticketDetail: {
        ticketStatus: 'Open',
        ticketSubTypeId: 1,
        ticketSubTypeName: 'test',
        ticketId: 123,
        typeName: 'abc',
        recipients: [
          {
            groupName: null,
            nickName: 'sa_nickname',
            recipientId: 8,
            recipientType: 1,
            userName: 'sa'
          }
        ]
      }
    };
    render(<TicketDetailInformation {...props} />);
    const recipient = screen.getAllByText('sa');
    expect(recipient).toBeTruthy();
  });
});
