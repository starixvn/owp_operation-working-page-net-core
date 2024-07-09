import store from '../../app/storeNative';
import { handleNewMessageIncome } from './newMessageIncomeHandler';
import { reloadTicketCenterTable } from './signalRHelper';


jest.mock('./signalRHelper', () => ({
  reloadTicketCenterTable: jest.fn()
}));
jest.mock('../../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

store.getState = () => ({
  signalR:{
    filterModel:{},
    issueModel:{}
  },
  chatBoard:{
    isOpenPopup: false,
    ticketDetail:{
      ticketId: 123
    }
  }
});


describe('handleNewMessageIncome under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    handleNewMessageIncome({
      newMsgInfo: {
        ticketId: 123
      }
    });

    expect(store.dispatch).toBeCalled();
  });
});
