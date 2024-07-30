import store from '../app/storeNative';
import { reloadTicketCellTable } from './signalRHelper';

const mockDispatch = jest.fn();

jest.mock('../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

store.dispatch = mockDispatch;
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

describe('SignalRWrapper under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call dispatch', () => {
    reloadTicketCellTable({
      owpMatchId: 1234,
      actorId: 8
    });

    expect(mockDispatch).toBeCalledTimes(2);
  });
});
