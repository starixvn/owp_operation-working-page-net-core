import store from '../app/store';
import { updateSubTypeTicketDetailByTicketId } from '../../Shared/chatBoardNew/chatBoardSlice';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';

jest.mock('../app/store', () => ({
  dispatch: jest.fn()
}));

describe('changeSubTypeIncomeHandler', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch updateSubTypeTicketDetailByTicketId action with correct ticketInfo', () => {
    const ticketInfo = {
      ticketId: 'mockedTicketId',
      subType: 'mockedSubType'
    };

    changeSubTypeIncomeHandler(ticketInfo);

    expect(store.dispatch).toHaveBeenCalledWith(
      updateSubTypeTicketDetailByTicketId(ticketInfo)
    );
  });
});
