import store from '../app/store'; // Đảm bảo import đúng store của ứng dụng của bạn
import notifyChangedInformationData from './notifyChangedInformationData';
import { setIsBlinkTitle } from '../../Shared/chatBoardNew/chatBoardSlice';

jest.mock('../app/store', () => ({
  getState: jest.fn(),
  dispatch: jest.fn()
}));

describe('notifyChangedInformationData', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should set IsBlinkTitle to true in production environment with matching ticketId and isNewMessage', () => {
    process.env.NODE_ENV = 'production';

    const mockedTicketId = 'mockedTicketId';
    const mockedData = {
      message: {
        ticketId: mockedTicketId,
        isNewMessage: true
      }
    };

    store.getState.mockReturnValue({
      chatBoard: {
        ticketDetail: {
          ticketId: mockedTicketId
        }
      }
    });

    notifyChangedInformationData(mockedData);

    expect(store.dispatch).toHaveBeenCalledWith(setIsBlinkTitle(true));
  });

  it('should not dispatch action if conditions are not met', () => {
    process.env.NODE_ENV = 'production';

    const mockedData = {
      message: {
        ticketId: 'mockedTicketId',
        isNewMessage: false
      }
    };

    store.getState.mockReturnValue({
      chatBoard: {
        ticketDetail: {
          ticketId: 'mockedTicketId'
        }
      }
    });

    notifyChangedInformationData(mockedData);

    expect(store.dispatch).not.toHaveBeenCalled();
  });
});
