import store from '../../app/storeNative';
import { reloadTicketCenterTable } from './signalRHelper';

const mockDispatch = jest.fn();

jest.mock('../../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

store.dispatch = mockDispatch;


describe('SignalRWrapper under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    reloadTicketCenterTable({
      signalR: {
        filterModel: {},
        issueModel: {}
      },
      chatBoard: {
        isOpenPopup: true
      }
    });

    expect(mockDispatch).toBeCalledTimes(2);
  });
});
