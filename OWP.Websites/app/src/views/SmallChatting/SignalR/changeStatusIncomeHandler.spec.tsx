import store from '../app/store';
import { setTicketStatus as setPopupTicketStatus } from '../../Shared/chatBoardNew/chatBoardSlice';
import changeStatusIncomeHandler from './changeStatusIncomeHandler';

jest.mock('../app/store', () => ({
  getState: jest.fn(),
  dispatch: jest.fn()
}));

describe('changeStatusIncomeHandler', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch setPopupTicketStatus action with correct ticketStatus when ticketId matches', () => {
    const ticketInfo = {
      ticketId: 'mockedTicketId',
      ticketStatus: 'mockedStatus'
    };

    const mockedState = {
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    };

    (store.getState as jest.Mock).mockReturnValue(mockedState);

    changeStatusIncomeHandler(ticketInfo);

    expect(store.dispatch).toHaveBeenCalledWith(
      setPopupTicketStatus(ticketInfo.ticketStatus)
    );
  });

  it('should not dispatch setPopupTicketStatus action when ticketId does not match', () => {
    const ticketInfo = {
      ticketId: 'mockedTicketId2',
      ticketStatus: 'mockedStatus'
    };

    const mockedState = {
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    };

    (store.getState as jest.Mock).mockReturnValue(mockedState);

    changeStatusIncomeHandler(ticketInfo);

    expect(store.dispatch).not.toHaveBeenCalled();
  });
});
