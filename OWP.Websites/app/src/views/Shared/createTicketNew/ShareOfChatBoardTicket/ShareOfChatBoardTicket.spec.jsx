import ShareOfChatBoardTicket from './ShareOfChatBoardTicket';
import { useSelector, useDispatch } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';

const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const recipient = [
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
];

const subTypeOption = [
  { value: null, label: '' },
  { value: 1, label: 'abc' },
  { value: 2, label: 'xyz' }
];

const mockHandleOnSubmit = jest.fn();
const mockHandleOnClosePopup = jest.fn();
const mockSetSelectedFiles = jest.fn();

const props = {
  handleOnSubmit: mockHandleOnSubmit,
  handleOnClosePopup: mockHandleOnClosePopup,
  selectedFiles: [],
  setSelectedFiles: mockSetSelectedFiles,
  ticketType: 1
};

const setup = (errorRecipient, selectedRecipients, subTypeOption) => {
  const initialState = {
    createTicketBoard: {
      errorRecipient: errorRecipient,
      selectedRecipients: selectedRecipients,
      subTypeOption: subTypeOption
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
};

describe('ShareOfEventOther under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should called when click', () => {
    setup(false, recipient, subTypeOption);
    const { container } = render(<ShareOfChatBoardTicket {...props} />);
    const buttonCancel = container.querySelector('.button-close');
    fireEvent.click(buttonCancel);
    const buttonSubmit = container.querySelector('.button-submit');
    fireEvent.click(buttonSubmit);
    expect(mockHandleOnClosePopup).toBeCalled();
    expect(mockHandleOnSubmit).toBeCalled();
  });
});
