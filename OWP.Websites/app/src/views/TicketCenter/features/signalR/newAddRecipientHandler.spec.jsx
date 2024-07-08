import store from '../../app/storeNative';
import { handleNewAddRecipient } from './newAddRecipientHandler';
import {reloadTicketCenterTable} from './signalRHelper';

jest.mock('./signalRHelper', () => ({
  reloadTicketCenterTable:jest.fn(),
  checkAllFilterCondition: jest.fn().mockReturnValue(true)
}));

jest.mock('../../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn(),
  getState: jest.fn()
}));

store.getState = () => ({
  signalR:{
    filterModel:{}
  }
});



describe('handleNewAddRecipient under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    handleNewAddRecipient();

    expect(store.dispatch).toBeCalledTimes(1);
  });
});
