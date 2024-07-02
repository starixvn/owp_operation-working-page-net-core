import store from '../../app/storeNative';
import { handleNewMessageIncome as handleNewMessageIncome } from './newMessageIncomeHandler';
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

store.getState = () => { };


describe('handleNewMessageIncome under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    handleNewMessageIncome();

    expect(reloadTicketCenterTable).toBeCalledTimes(1);
  });
});
