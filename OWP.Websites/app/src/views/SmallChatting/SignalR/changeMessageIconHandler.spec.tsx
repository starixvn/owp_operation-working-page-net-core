import store from '../app/store';
import { updateMessageIconRealTime } from '../../Shared/chatBoardNew/chatBoardSlice';
import { changeMessageIconHandler } from './changeMessageIconHandler';

jest.mock('../app/store', () => ({
  getState: jest.fn(),
  dispatch: jest.fn()
}));

describe('changeMessageIconHandler', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch updateMessageIconRealTime action when ticketId matches', () => {
    const messageReactIcon = 'like';
    const action = 'add';
    const ticketId = 'mockedTicketId';

    const mockedState = {
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    };

    (store.getState as jest.Mock).mockReturnValue(mockedState);

    changeMessageIconHandler(messageReactIcon, action, ticketId);

    expect(store.dispatch).toHaveBeenCalledWith(
      updateMessageIconRealTime({
        messageReactIcon,
        action
      })
    );
  });

  it('should not dispatch updateMessageIconRealTime action when ticketId does not match', () => {
    const messageReactIcon = 'like';
    const action = 'add';
    const ticketId = 'mockedTicketId2';

    const mockedState = {
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    };

    (store.getState as jest.Mock).mockReturnValue(mockedState);

    changeMessageIconHandler(messageReactIcon, action, ticketId);

    expect(store.dispatch).not.toHaveBeenCalled();
  });
});
