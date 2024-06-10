import { REQUEST_STATUS } from '../../../Shared/Constants';
import {
  FilterState,
  MatchInfo,
  PopupState,
  RefreshModel
} from '../../type/TableType';
import reducer, {
  filterBySearchKey,
  clearFilter,
  setDataTable,
  setDataSignalR,
  setUserId,
  filterByUnReadTicket,
  openPopupDetail,
  setRefreshModel,
  closePopupDetail
} from './tableSlice';
import { ORDER_ASC, ORDER_DESC, KOTime } from '../TableSchema.json';
import { getEvents } from './tableThunk';
import { fakeEntities } from './tableSliceConst';
describe('tableSlice', () => {
  const initialState = {
    loading: REQUEST_STATUS.idle,
    isShowUnReadTicketOnly: false,
    entitiesOriginal: [] as MatchInfo[],
    entities: [] as MatchInfo[],
    userId: 0 as number,
    popupState: {
      matchId: undefined,
      isOpen: false
    } as PopupState,
    sortState: {
      columnKey: KOTime.columnKey,
      orderType: '',
    },
    filterState: [] as FilterState[],
    refreshModel: {} as RefreshModel
  };

  it('return init state', () => {
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initialState);
  });

  it('should handle when getEvents pending', () => {
    const action = {
      type: getEvents.pending
    };
    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      loading: REQUEST_STATUS.pending
    });
  });

  it('should handle when getEvents fulfilled', () => {
    const action = {
      type: getEvents.fulfilled.type,
      payload: []
    };

    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      loading: REQUEST_STATUS.idle
    });
  });

  it('should handle when getEvents fulfilled', () => {
    const action = {
      type: getEvents.rejected.type,
      payload: []
    };

    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      loading: REQUEST_STATUS.idle
    });
  });

  it('setRefreshModel', () => {
    const action = {
      type: setRefreshModel.type,
      payload: {
        filterDate: new Date(),
        statusId: 'loading'
      }
    };

    const state = reducer(initialState, action);
    expect(state.refreshModel).toEqual(action.payload);
  });

  it('closePopupDetail', () => {
    const action = {
      type: closePopupDetail.type
    };

    const state = reducer(initialState, action);
    expect(state.popupState).toEqual({
      matchId: undefined,
      isOpen: false
    });
  });

  it('openPopupDetail', () => {
    const action = {
      type: openPopupDetail.type,
      payload: 123
    };

    const state = reducer(initialState, action);
    expect(state.popupState).toEqual({
      matchId: 123,
      isOpen: true
    });
  });

  it('setDataSignalR', () => {
    const action = {
      type: setDataSignalR.type,
      payload: {
        actorId: 8,
        owpMatchId: 930265
      }
    };

    const state = reducer(
      {
        ...initialState,
        entities: [
          {
            ...fakeEntities[0],
            IsRead: true
          }
        ],
        userId: 11224
      },
      action
    );
    expect(state.entities[0].IsRead).toEqual(false);
  });

  it('setDataTable', () => {
    const action = {
      type: setDataTable.type,
      payload: [
        {
          League: 'INDIA DELHI PREMIER LEAGUE',
          OWPMatchId: 930265
        }
      ]
    };

    const state = reducer(initialState, action);
    expect(state.entities[0].OWPMatchId).toEqual(930265);
  });

  it('setUserId', () => {
    const action = {
      type: setUserId.type,
      payload: 8
    };

    const state = reducer(initialState, action);
    expect(state.userId).toEqual(8);
  });

  it('clearFilter', () => {
    const action = {
      type: clearFilter.type
    };

    const state = reducer(
      {
        ...initialState,
        entitiesOriginal: fakeEntities
      },
      action
    );
    expect(state.entities).toEqual(fakeEntities);
  });

  it('filterByUnReadTicket', () => {
    const action = {
      type: filterByUnReadTicket.type,
      payload: true
    };
    const state = reducer(
      {
        ...initialState,
        entitiesOriginal: [
          {
            ...fakeEntities[0],
            IsRead: true
          }
        ]
      },
      action
    );
    expect(state.entities).toEqual([]);
  });
});
