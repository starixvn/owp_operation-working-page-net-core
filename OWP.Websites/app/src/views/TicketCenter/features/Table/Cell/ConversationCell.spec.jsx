import ConversationCell from './ConversationCell';
import { fireEvent, render, screen } from '@testing-library/react';
import { getState } from '../../../app/storeNative';
import {
  useStableDispatch,
  updatedIsReadOneRow
} from '../../../ImportTicketCenterHelperNative';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../../../../Shared/chatBoardNew/chatBoardSlice';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

const middleWares = [];
const mockStore = configureStore(middleWares);

jest.mock('../../../app/storeNative', () => ({
  getState: jest.fn()
}));
getState.mockImplementation(() => {
  return {
    pagePolicy: {
      permission: {
        ActorId: {
          HasWrite: true
        }
      }
    }
  };
});
jest.mock('../../../../Shared/chatBoardNew/chatBoardSlice', () => ({
  getTicketDetail: jest.fn().mockImplementation(() => {}),
  getTicketSubTypes: jest.fn()
}));

jest.mock('../../../ImportTicketCenterHelperNative', () => ({
  useStableDispatch: jest.fn(),
  updatedIsReadOneRow: jest.fn()
}));
useStableDispatch.mockImplementation(() => jest.fn(() => jest.fn(() => null)));
const props = {
  rowIndex: 0,
  dataSource: [{ TicketId: 1, ticketStatus: 'Cancel', isRead: true }]
};

jest.mock('../../../app/storeNative.tsx');

const store = mockStore({
  tableReducer: {
    filterDate: 'abc'
  }
});

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('ConversationCell under test', () => {
  it('should render TicketIdCell', () => {
    getTicketDetail.rejected = 'abc';

    const { container } = render(<Wrapper>
      <ConversationCell {...props} /></Wrapper>);
    const element = container.querySelector(
      `[class="new-add-league-name ticket-title-link"]`
    );
  
    expect(element).toBeInTheDocument();
   
  });
});
