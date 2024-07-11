import store from '../app/store';
import { setNewMessage as setNewMessageOnChatBoard } from '../../Shared/chatBoardNew/chatBoardSlice';
import handleNewMessageIncome from './newMessageIncomeHandler';

jest.mock('../app/store', () => ({
  getState: jest.fn(),
  dispatch: jest.fn()
}));

describe('handleNewMessageIncome', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch setNewMessageOnChatBoard action when ticketId matches', () => {
    const mockedTicketId = 'mockedTicketId';
    const newMsgInfo = {
      ticketId: mockedTicketId,
      message: 'New message'
    };

    store.getState.mockReturnValue({
      chatBoard: {
        ticketDetail: {
          ticketId: mockedTicketId
        }
      }
    });

    handleNewMessageIncome(newMsgInfo, {});

    expect(store.dispatch).toHaveBeenCalledWith(
      setNewMessageOnChatBoard(newMsgInfo)
    );
  });

  it('should not dispatch action if ticketId does not match', () => {
    const newMsgInfo = {
      ticketId: 'otherTicketId',
      message: 'New message'
    };

    store.getState.mockReturnValue({
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    });

    handleNewMessageIncome(newMsgInfo, {});

    expect(store.dispatch).not.toHaveBeenCalled();
  });
});
