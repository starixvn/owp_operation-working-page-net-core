import store from '../../app/storeNative';
import changeStatusIncomeHandler from './changeStatusIncomeHandler';
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
    changeStatusIncomeHandler();

    expect(reloadTicketCenterTable).toBeCalledTimes(1);
  });
});
