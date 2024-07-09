import ActionCell from './ActionCell';
import { fireEvent, render } from '@testing-library/react';
import {
  useStableDispatch,
  pinTicket,
  refreshEventTickets
} from '../../../ImportTicketCenterHelperNative';
import { getState } from '../../../app/storeNative';
import ToastsWrapper from '../../../../components/ToastrWrapper';

jest.mock('../../../ImportTicketCenterHelperNative', () => ({
  useStableDispatch: jest.fn(),
  refreshEventTickets: jest.fn(),
  pinTicket: jest.fn(),
  deleteTicket: jest.fn()
}));

jest.mock('../../../app/storeNative', () => ({
  getState: jest.fn()
}));

jest.mock('../../../../components/ToastrWrapper');

ToastsWrapper.confirm = jest.fn().mockReturnValue('Ok');

getState.mockImplementation(() => {
  return {
    pagePolicy: {
      permission: {
        ActorId: 8
      }
    },
    deletePolicy: {
      permission: {
        HasWrite: true
      }
    },
    signalR: {
      filterModel: {}
    }
  };
});

useStableDispatch.mockImplementation(() => jest.fn(() => jest.fn(() => null)));
const props = {
  rowIndex: 0,
  dataSource: [{ TicketId: 1, isRead: true, isPinned: true }]
};
describe('ActionCell under test', () => {
  it('should render TicketIdCell', () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="action-cell"]`);
    expect(element).toBeInTheDocument();
  });

  it('should handle-unpin TicketIdCell', () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="icon-unpin"]`);
    fireEvent.click(element);
    expect(element).toBeInTheDocument();
    expect(useStableDispatch).toBeCalled();
    expect(pinTicket).toBeCalled();
    expect(refreshEventTickets).toBeCalled();
  });

  it('should deleteTicket TicketIdCell', () => {
    const { container } = render(<ActionCell {...props} />);
    const element = container.querySelector(`[class="icon-delete"]`);
    fireEvent.click(element);
    expect(element).toBeInTheDocument();
    expect(useStableDispatch).toBeCalled();
  });
});
