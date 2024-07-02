import store from '../../app/storeNative';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
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


describe('changeStatusIncomeHandler under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    changeSubTypeIncomeHandler();

    expect(reloadTicketCenterTable).toBeCalledTimes(1);
  });
});
