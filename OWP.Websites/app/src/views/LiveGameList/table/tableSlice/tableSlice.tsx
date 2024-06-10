import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { ORDER_ASC, ORDER_DESC } from '../TableSchema.json';
import {
  FilterState,
  ISortState,
  MatchInfo,
  PopupState,
  RefreshModel,
  FilterAllColumn
} from '../../type/TableType';
import {
  filterData,
  sortDataAsc,
  sortDataDesc
} from '../LiveGameListTableHelper';
import { getEvents } from './tableThunk';
import { buildIsReadValue } from '../../../Shared/ticketEvent/helper/ticketHelper';
import {
  addOrReplaceFilterState,
  pushOrUpdateObject
} from '../../../Shared/Utils';
import { isEqual } from 'lodash';

const tableSlice = createSlice({
  name: 'EventInformation',
  initialState: {
    valueFilterAllColumn: {} as FilterAllColumn,
    columnKeyFilterCurrent: '',
    isShowFilterHeader: false,
    loading: REQUEST_STATUS.idle,
    isShowUnReadTicketOnly: false,
    entitiesOriginal: [] as MatchInfo[],
    entities: [] as MatchInfo[],
    userId: 0 as number,
    popupState: {
      matchId: undefined,
      isOpen: false
    } as PopupState,
    filterState: [] as FilterState[],
    sortState: {
      columnKey: 'KO Time',
      orderType: ''
    } as ISortState,
    refreshModel: {} as RefreshModel
  },
  reducers: {
    setValueFilter: (state, action) => {
      const { valueFilterAllColumn } = state;
      state.valueFilterAllColumn = pushOrUpdateObject(
        valueFilterAllColumn,
        action.payload
      );
    },
    setColumnKeyValue: (state, action) => {
      state.columnKeyFilterCurrent = action.payload;
    },
    filterByUnReadTicket: (state, action) => {
      const { entitiesOriginal, filterState } = state;
      state.isShowUnReadTicketOnly = action.payload;
      state.entities = filterData(entitiesOriginal)(
        state.isShowUnReadTicketOnly
      )(filterState);
      const { orderType } = state.sortState;
      if (isEqual(orderType, ORDER_ASC)) {
        state.entities = sortDataAsc(state.entities);
      }
      if (isEqual(orderType, ORDER_DESC)) {
        state.entities = sortDataDesc(state.entities);
      }
    },
    filterBySearchKey: (state, action) => {
      const { entitiesOriginal, filterState, isShowUnReadTicketOnly } = state;
      state.filterState = addOrReplaceFilterState(filterState, action.payload);
      state.entities = filterData(entitiesOriginal)(isShowUnReadTicketOnly)(
        state.filterState
      );
      const { orderType } = state.sortState;
      if (isEqual(orderType, ORDER_ASC)) {
        state.entities = sortDataAsc(state.entities);
      }
      if (isEqual(orderType, ORDER_DESC)) {
        state.entities = sortDataDesc(state.entities);
      }
    },
    clearFilter: state => {
      const { entitiesOriginal } = state;
      state.filterState = [];
      state.valueFilterAllColumn = {};
      state.entities = entitiesOriginal;
      const { orderType } = state.sortState;
      if (isEqual(orderType, ORDER_ASC)) {
        state.entities = sortDataAsc(state.entities);
      }
      if (isEqual(orderType, ORDER_DESC)) {
        state.entities = sortDataDesc(state.entities);
      }
    },
    setUserId: (state, action) => {
      state.userId = action.payload;
    },
    setDataTable: (state, action) => {
      state.entities = action.payload;
    },

    //TODO
    setDataSignalR: (state, action) => {
      const { actorId, parentMatchId } = action.payload;
      state.entities.forEach(item => {
        if (item.ParentMatchId === parentMatchId) {
          item.IsRead = buildIsReadValue(actorId, state.userId, item.IsRead);
          item.HasTicket = true;
        }
      });
    },
    openPopupDetail: (state, action) => {
      state.popupState = {
        matchId: action.payload,
        isOpen: true
      };
    },
    closePopupDetail: state => {
      state.popupState = {
        matchId: undefined,
        isOpen: false
      };
    },
    setRefreshModel: (state, action) => {
      state.refreshModel = action.payload;
    },
    sortFunc: (state, action) => {
      state.sortState = action.payload;
      const { orderType } = action.payload;
      if (isEqual(orderType, ORDER_ASC)) {
        state.entities = sortDataAsc(state.entities);
      }
      if (isEqual(orderType, ORDER_DESC)) {
        state.entities = sortDataDesc(state.entities);
      }
    }
  },

  extraReducers: builder => {
    builder
      .addCase(getEvents.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getEvents.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        const { filterState, isShowUnReadTicketOnly } = state;
        state.entitiesOriginal = payload;
        state.entities = filterData(payload)(isShowUnReadTicketOnly)(
          filterState
        );
        const { orderType } = state.sortState;
        if (isEqual(orderType, ORDER_ASC)) {
          state.entities = sortDataAsc(state.entities);
        }
        if (isEqual(orderType, ORDER_DESC)) {
          state.entities = sortDataDesc(state.entities);
        }
      })
      .addCase(getEvents.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});

const { actions, reducer } = tableSlice;
const {
  setValueFilter,
  setColumnKeyValue,
  filterBySearchKey,
  clearFilter,
  setDataTable,
  setDataSignalR,
  setUserId,
  filterByUnReadTicket,
  openPopupDetail,
  setRefreshModel,
  closePopupDetail,
  sortFunc
} = actions;
export {
  setValueFilter,
  setColumnKeyValue,
  filterBySearchKey,
  clearFilter,
  setDataTable,
  setDataSignalR,
  setUserId,
  filterByUnReadTicket,
  openPopupDetail,
  setRefreshModel,
  closePopupDetail,
  sortFunc
};
export default reducer;
