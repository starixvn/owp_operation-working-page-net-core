import slice, { getTicketList, setTicketList } from './ticketListSlice';

describe('unit test for ticketListSlice', () => {
  it('should return init state', () => {
    const action = {};
    const expectedState = {
      loading: false,
      ticketList: []
    };

    const newState = slice(undefined, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle setTicketList', () => {
    const state = {
      ticketList: []
    };
    const expectedState = {
      ticketList: [
        {
          TicketId: 123,
          RecipientId: 123
        }
      ]
    };
    const action = {
      type: setTicketList.type,
      payload: [
        {
          TicketId: 123,
          RecipientId: 123
        }
      ]
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle getTicketList.pending', () => {
    const state = {
      loading: false
    };
    const expectedState = {
      loading: true
    };
    const action = {
      type: getTicketList.pending
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle getTicketList.fulfilled', () => {
    const state = {
      loading: true,
      ticketList: []
    };
    const expectedState = {
      loading: false,
      ticketList: [
        {
          TicketId: 123,
          RecipientId: 123
        }
      ]
    };
    const action = {
      type: getTicketList.fulfilled,
      payload: [
        {
          TicketId: 123,
          RecipientId: 123
        }
      ]
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
});
