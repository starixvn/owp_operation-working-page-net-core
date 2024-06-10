import { render } from '@testing-library/react';
import store from '../../app/storeNative';
import SignalRWrapper from './useSignalRWrapper';

import Spinner from '../../../components/Spinner';
import {
  getEventTickets,
  refreshEventTickets
} from '../../ImportTicketCenterHelperNative';

const mockDispatch = jest.fn();
const getEventTicketsMock = jest.fn();
const refreshEventTicketsMock = jest.fn();
jest.mock('../../../components/Spinner');
jest.mock('../../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
const mockState = {
  chatBoard: {
    isOpenPopup: false
  },
  signalR: {
    filterModel: { MarketKey: 1, ActorId: 169368 },
    issueModel: {
      ticketId: 566,
      actorId: 169368,
      detailSegmentUrl: 'ticket-detail/get-other',
      eventType: 3
    }
  }
};
store.getState = () => mockState;
store.dispatch = () => mockDispatch;

jest.mock('../../ImportTicketCenterHelperNative', () => ({
  getEventTickets: jest.fn(),
  refreshEventTickets: jest.fn()
}));
Spinner.show = jest.fn().mockReturnValue('OK');
getEventTickets.mockReturnValue(getEventTicketsMock);
refreshEventTickets.mockReturnValue(refreshEventTicketsMock);

describe('SignalRWrapper under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have SignalR', () => {
    render(<SignalRWrapper />);

    expect(Spinner.show).toBeCalledTimes(1);
  });
});
