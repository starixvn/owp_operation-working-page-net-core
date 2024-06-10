import TicketLogWorkCreator from './TicketLogWorkCreator';
import { useSelector } from 'react-redux';
import { LogWorkProps } from './Type';
import { render, screen } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
const state = {
  createTicketBoard: {
    isOpenLogWork: true
  }
};
(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({ ...state })
);
const props: LogWorkProps = {
  creatorInfo: {
    groupName: null,
    nickName: 'test1',
    recipientId: 123,
    recipientType: 1,
    userName: 'test1'
  }
};

describe('Subject', () => {
  it('Ticket Log Work', () => {
    render(<TicketLogWorkCreator creatorInfo={props.creatorInfo} />);
    const title = screen.getByText('Type: S&VR');
    expect(title).toBeInTheDocument();
  });
});
