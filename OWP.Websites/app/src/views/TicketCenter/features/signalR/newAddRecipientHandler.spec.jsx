import store from '../../app/storeNative';
import { handleNewAddRecipient } from './newAddRecipientHandler';
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


describe('handleNewAddRecipient under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    handleNewAddRecipient();

    expect(reloadTicketCenterTable).toBeCalledTimes(1);
  });
});
