import { REQUEST_STATUS } from '../../../../Shared/Constants';
import eventCheckingSlice, {
  sort,
  updatedIsReadOneRow,
  showUnreadMessageOnly,
  updatedIsReadAllRows
} from './eventCheckingSlice';
import {
  getEventTickets,
  refreshEventTickets,
  pinTicket,
  deleteTicket
} from './eventSliceThunkNative';

jest.mock('../../../app/storeNative');
let initialState = null;

afterEach(() => {
  initialState = null;
});
describe('eventCheckingSlice under test', () => {
  it('should return initial state', () => {
    expect(eventCheckingSlice(undefined, {})).toEqual({
      entities: [],
      entitiesOrigin: [],
      currentRequestId: undefined,
      loading: REQUEST_STATUS.idle,
      error: null,
      sortState: {},
      isShowUnreadMessOnly: false,
      ticketType: [{ value: 0, label: 'All' }]
    });
  });

  describe('getEventTickets under test', () => {
    it('initialState loading is idle, should handle getEventTickets.pending', () => {
      initialState = {
        currentRequestId: undefined,
        loading: REQUEST_STATUS.idle
      };

      const expectedState = {
        currentRequestId: 1,
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getEventTickets.pending.type,
        meta: { requestId: 1 }
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
    it('initialState loading is pending, should handle getEventTickets.pending', () => {
      initialState = {
        currentRequestId: undefined,
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getEventTickets.pending.type,
        meta: { requestId: 1 }
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('should handle getEventTickets.fulfilled case "requestId === currentRequestId"', () => {
      initialState = {
        entities: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.pending,
        isHideDoneIssues: false,
        error: null
      };

      const expectedState = {
        entities: [1, 2, 3],
        entitiesOrigin: [1, 2, 3],
        loading: REQUEST_STATUS.idle,
        currentRequestId: null,
        isHideDoneIssues: false,
        error: null
      };

      const action = {
        type: getEventTickets.fulfilled.type,
        meta: { requestId: 2 },
        payload: [1, 2, 3]
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('should handle getEventTickets.fulfilled case "requestId !== currentRequestId"', () => {
      initialState = {
        entities: [],
        entitiesOrigin: [],
        currentRequestId: 3,
        loading: REQUEST_STATUS.pending,
        isChecked: false,
        error: null
      };

      const expectedState = {
        entities: [],
        entitiesOrigin: [],
        loading: REQUEST_STATUS.pending,
        currentRequestId: 3,
        isChecked: false,
        error: null
      };

      const action = {
        type: getEventTickets.fulfilled.type,
        meta: { requestId: 2 },
        payload: [1, 2, 3]
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('initialState loading is pending, should handle getEventTickets.rejected', () => {
      initialState = {
        entities: [],
        entitiesOrigin: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.pending,
        isChecked: false,
        error: null
      };

      const expectedState = {
        entities: [],
        entitiesOrigin: [],
        loading: REQUEST_STATUS.idle,
        currentRequestId: null,
        isChecked: false,
        error: 'test'
      };

      const action = {
        type: getEventTickets.rejected.type,
        meta: { requestId: 2 },
        payload: { id: 1, test: [2, 3] },
        error: 'test'
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
    it('initialState loading is idle, should handle getEventTickets.rejected', () => {
      initialState = {
        entities: [],
        entitiesOrigin: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.idle,
        isChecked: false,
        error: null
      };

      const action = {
        type: getEventTickets.rejected.type,
        meta: { requestId: 2 },
        payload: { id: 1, test: [2, 3] },
        error: 'test'
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(initialState);
    });
  });

  describe('sort under test', () => {
    it('should handle sort ORDER_DESC', () => {
      initialState = {
        entities: [
          {
            LeagueId: 1,
            LeagueName: 'Premier League'
          },
          {
            LeagueId: 2,
            LeagueName: 'Laliga'
          },
          {
            LeagueId: 3,
            LeagueName: 'League 1'
          },
          {
            LeagueId: 4,
            LeagueName: 'Seria'
          }
        ],
        sortState: {},
        loading: REQUEST_STATUS.idle
      };

      const expectedState = {
        entities: [
          {
            LeagueId: 4,
            LeagueName: 'Seria'
          },
          {
            LeagueId: 3,
            LeagueName: 'League 1'
          },
          {
            LeagueId: 2,
            LeagueName: 'Laliga'
          },
          {
            LeagueId: 1,
            LeagueName: 'Premier League'
          }
        ],
        sortState: { columnKey: 'LeagueId', orderType: 'ORDER_DESC' },
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: sort.type,
        payload: { columnKey: 'LeagueId', orderType: 'ORDER_DESC' }
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('should handle sort ORDER_ASC', () => {
      initialState = {
        entities: [
          {
            LeagueId: 1,
            LeagueName: 'Premier League'
          },
          {
            LeagueId: 2,
            LeagueName: 'Laliga'
          },
          {
            LeagueId: 3,
            LeagueName: 'League 1'
          },
          {
            LeagueId: 4,
            LeagueName: 'Seria'
          }
        ],
        sortState: {},
        loading: REQUEST_STATUS.idle
      };

      const expectedState = {
        entities: [
          {
            LeagueId: 1,
            LeagueName: 'Premier League'
          },
          {
            LeagueId: 2,
            LeagueName: 'Laliga'
          },
          {
            LeagueId: 3,
            LeagueName: 'League 1'
          },
          {
            LeagueId: 4,
            LeagueName: 'Seria'
          }
        ],
        sortState: { columnKey: 'LeagueId', orderType: 'ORDER_ASC' },
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: sort.type,
        payload: { columnKey: 'LeagueId', orderType: 'ORDER_ASC' }
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('showUnreadMessageOnly under test', () => {
    it('should handle showUnreadMessageOnly', () => {
      initialState = {
        entities: [],
        entitiesOrigin: [],
        isHideDoneIssue: false,
        isShowUnreadMessOnly: false
      };

      const expectedState = {
        entities: [],
        entitiesOrigin: [],
        isHideDoneIssue: false,
        isShowUnreadMessOnly: false
      };

      const action = {
        type: showUnreadMessageOnly.type,
        payload: false
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('should handle showUnreadMessageOnly', () => {
      initialState = {
        entities: [],
        entitiesOrigin: [
          {
            LeagueId: 1,
            isRead: 1
          },
          {
            LeagueId: 2,
            isRead: 0
          },
          {
            LeagueId: 3,
            isRead: 1
          },
          {
            LeagueId: 4,
            isRead: 0
          }
        ],
        isShowUnreadMessOnly: true
      };

      const expectedState = {
        entities: [
          {
            LeagueId: 2,
            isRead: 0
          },
          {
            LeagueId: 4,
            isRead: 0
          }
        ],
        entitiesOrigin: [
          {
            LeagueId: 1,
            isRead: 1
          },
          {
            LeagueId: 2,

            isRead: 0
          },
          {
            LeagueId: 3,
            isRead: 1
          },
          {
            LeagueId: 4,
            isRead: 0
          }
        ],
        isShowUnreadMessOnly: true
      };

      const action = {
        type: showUnreadMessageOnly.type,
        payload: true
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('updatedIsReadOneRow under test', () => {
    it('should handle updateIsReadOneRow', () => {
      initialState = {
        entities: [
          {
            ticketId: 1,
            isRead: false
          }
        ],
        entitiesOrigin: [
          {
            ticketId: 1,
            isRead: false
          }
        ]
      };

      const expectedState = {
        entities: [
          {
            isRead: true,
            ticketId: 1
          }
        ],
        entitiesOrigin: [
          {
            ticketId: 1,
            isRead: true
          }
        ]
      };

      const action = {
        type: updatedIsReadOneRow.type,
        payload: 0
      };

      const state = eventCheckingSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });
});

describe('updatedIsReadAllRows under test', () => {
  it('should handle updatedIsReadAllRows', () => {
    initialState = {
      entities: [
        {
          ticketId: 1,
          isRead: false
        },
        {
          ticketId: 2,
          isRead: false
        }
      ],
      entitiesOrigin: [
        {
          ticketId: 1,
          isRead: false
        },
        {
          ticketId: 2,
          isRead: false
        }
      ]
    };

    const expectedState = {
      entities: [
        {
          isRead: true,
          ticketId: 1
        },
        {
          ticketId: 2,
          isRead: true
        }
      ],
      entitiesOrigin: [
        {
          ticketId: 1,
          isRead: true
        },
        {
          ticketId: 2,
          isRead: true
        }
      ]
    };

    const action = {
      type: updatedIsReadAllRows.type,
      payload: 0
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });
});

describe('refreshEventTickets under test', () => {
  it('should handle refreshEventTickets.fulfilled', () => {
    initialState = {
      entities: [1, 2],
      entitiesOrigin: [3, 4],
      currentRequestId: 3,
      loading: REQUEST_STATUS.pending,
      isChecked: false,
      sortState: { columnKey: 'LeagueId', orderType: 'ORDER_DESC' },
      error: null
    };

    const expectedState = {
      entities: [1, 2, 3],
      entitiesOrigin: [1, 2, 3],
      loading: REQUEST_STATUS.pending,
      currentRequestId: 3,
      sortState: { columnKey: 'LeagueId', orderType: 'ORDER_DESC' },
      isChecked: false,
      error: null
    };

    const action = {
      type: refreshEventTickets.fulfilled.type,
      payload: [1, 2, 3]
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });
});

describe('pinTicket under test', () => {
  it('should handle pinTicket.pending', () => {
    initialState = {
      loading: REQUEST_STATUS.idle
    };

    const expectedState = {
      loading: REQUEST_STATUS.pending
    };

    const action = {
      type: pinTicket.pending.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });

  it('should handle pinTicket.reject', () => {
    initialState = {
      loading: REQUEST_STATUS.pending
    };

    const expectedState = {
      loading: REQUEST_STATUS.idle
    };

    const action = {
      type: pinTicket.rejected.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });

  it('should handle pinTicket.fulfilled', () => {
    initialState = {
      loading: null
    };

    const expectedState = {
      loading: REQUEST_STATUS.idle
    };

    const action = {
      type: pinTicket.fulfilled.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });
});

describe('deleteTicket under test', () => {
  it('should handle deleteTicket.pending', () => {
    initialState = {
      loading: REQUEST_STATUS.idle
    };

    const expectedState = {
      loading: REQUEST_STATUS.pending
    };

    const action = {
      type: deleteTicket.pending.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });

  it('should handle deleteTicket.reject', () => {
    initialState = {
      loading: REQUEST_STATUS.pending
    };

    const expectedState = {
      loading: REQUEST_STATUS.idle
    };

    const action = {
      type: deleteTicket.rejected.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });

  it('should handle deleteTicket.fulfilled', () => {
    initialState = {
      loading: null
    };

    const expectedState = {
      loading: REQUEST_STATUS.idle
    };

    const action = {
      type: deleteTicket.fulfilled.type
    };

    const state = eventCheckingSlice(initialState, action);

    expect(state).toEqual(expectedState);
  });
});
