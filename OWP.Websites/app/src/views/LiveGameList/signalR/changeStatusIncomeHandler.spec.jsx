import store from '../app/storeNative';
import { handleChangeStatus } from './changeStatusIncomeHandler';
import { handleReloadTicketDetail } from '../../Shared/ticketEvent/signalR/reLoadHelper';

jest.mock('../../Shared/ticketEvent/signalR/reLoadHelper', () => ({
  handleReloadTicketDetail: jest.fn()
}));
jest.mock('../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

store.getState = () => ({
  signalREventTicket: {
    issueModel: {
      ticketId: 123
    }
  },
  chatBoard: {
    isOpenPopup: true
  }
});
describe('changeStatusIncomeHandler under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    handleChangeStatus();

    expect(handleReloadTicketDetail).toBeCalledTimes(1);
  });
});
