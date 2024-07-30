import ticketReducer, {
  initialState,
  appendTicketItemIntoDataSource,
  updateTicketItemIntoDataSource
} from './ticketSlice';
import { getEventTickets } from './ticketSliceThunkNative';
import { REQUEST_STATUS } from '../../../../Shared/Constants';

describe('ticketSlice reducer', () => {
  it('should handle appendTicketItemIntoDataSource', () => {
    const newState = ticketReducer(
      initialState,
      appendTicketItemIntoDataSource({
        ticketId: '1'
      })
    );
    expect(newState.entities.length).toBe(1);
  });

  it('should handle getEventTickets.pending', () => {
    const newState = ticketReducer(
      initialState,
      getEventTickets.pending({ meta: { requestId: '123' } })
    );
    expect(newState.currentRequestId.meta.requestId).toContain('123');
  });

  it('should handle getEventTickets.fulfilled', () => {
    const ticketsPayload = [];
    const newState = ticketReducer(
      {
        ...initialState,
        loadTicket: REQUEST_STATUS.pending,
        currentRequestId: '123'
      },
      getEventTickets.fulfilled(ticketsPayload, { meta: { requestId: '123' } })
    );

    expect(newState.currentRequestId).toBe('123');
  });

  it('should handle getEventTickets.rejected', () => {
    const newState = ticketReducer(
      {
        ...initialState,
        loadTicket: REQUEST_STATUS.pending,
        currentRequestId: '123'
      },
      getEventTickets.rejected(undefined, { meta: { requestId: '123' } })
    );

    expect(newState.currentRequestId).toBe('123');
  });
});
