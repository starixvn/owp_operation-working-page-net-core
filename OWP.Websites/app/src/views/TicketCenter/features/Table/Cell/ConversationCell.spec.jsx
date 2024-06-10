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
describe('ConversationCell under test', () => {
  it('should render TicketIdCell', () => {
    getTicketDetail.rejected = 'abc';

    const { container } = render(<ConversationCell {...props} />);
    const element = container.querySelector(
      `[class="new-add-league-name ticket-title-link"]`
    );

    fireEvent.click(screen.getByRole('link'));

    expect(element).toBeInTheDocument();
    expect(useStableDispatch).toBeCalled();
    expect(getTicketDetail).toBeCalled();
    expect(getTicketSubTypes).toBeCalled();
    expect(updatedIsReadOneRow).toBeCalled();
    expect(getState).toBeCalled();
  });
});
