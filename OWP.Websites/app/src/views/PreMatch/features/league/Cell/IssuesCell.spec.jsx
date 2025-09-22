import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import IssuesCell from './IssuesCell';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
const setup = () => {
  const state = {
    policy: {
      permission: {
        HasWrite: true
      }
    },
    createTicketBoard: {
      defaultRecipients: []
    }
  };
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};
const createProps = HasMessages => {
  const dataSource = [
    { InfoSiteMatchId: 1, PreMatchIssueId: 999, HasMessages }
  ];
  const rowIndex = 0;
  const handleShowTicket = jest.fn();
  return { dataSource, rowIndex, handleShowTicket };
};

describe('IssuesCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when not have message, should render IconCreateMessage', () => {
    setup();
    const props = createProps(false);
    const { container } = render(<IssuesCell {...props} />);
    const button = container.querySelector('.icon-plus');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(8);
  });

  it('when have message, should render ', () => {
    setup();
    const props = createProps(true);
    const { container } = render(<IssuesCell {...props} />);
    const button = container.querySelector('.icon-unread-message');
    expect(button).toBeInTheDocument();
  });
});
