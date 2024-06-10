/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import getOSAssignment from './getOSAssignmentThunkNative';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import {
  filterHomeOrAwayByKeyword,
  updatePropHidenTranderMaxbet,
  findAndUpdatePropHidenTranderMaxbetByosMatchId
} from '../osTableHelper';
import { Trader } from '../TableSchema.json';
import { expand } from '../../../../Shared/common/CustomTable/commonCell/collapseExpandCell/collapseExpandConst.json';
import { orderBy } from '../orderHelpper';
import {
  addOrReplaceFilterState,
  filterDataTable
} from '../../../../Shared/Utils';

const osLiveGameAssignmentSlice = createSlice({
  name: 'osLiveGameAssignment',
  initialState: {
    editOfferLiveMatches: [],
    isEdit: false,
    entities: [],
    entitiesOriginal: [],
    sortState: {
      columnKey: undefined, // NOSONAR
      orderType: undefined // NOSONAR
    },
    filterState: [],
    collapseExpandAllRowState: {
      columnKey: Trader.columnKey,
      mode: expand
    },
    collapseExpandRowState: {
      columnKey: Trader.columnKey,
      mode: expand,
      rowHasModeCollapse: []
    },
    loading: REQUEST_STATUS.idle
  },
  reducers: {
    clearEditOfferLiveMatches: state => {
      state.editOfferLiveMatches = [];
    },
    cancelEditOfferLive: state => {
      state.entities = filterDataTable(
        state.entitiesOriginal,
        state.filterState,
        filterHomeOrAwayByKeyword
      );
      state.entities = orderBy(
        state.entitiesOriginal,
        state.sortState.columnKey,
        state.sortState.orderType
      );
    },
    upDateDataEntities: (state, action) => {
      const { entities, editOfferLiveMatches } = state;
      const { osMatchId, value } = action.payload;
      state.entities = entities.map(obj => {
        if (obj.osMatchId === osMatchId) {
          const editOfferLiveMatch = {
            osMatchId: obj.osMatchId,
            newOfferLive: value
          };
          editOfferLiveMatches.push(editOfferLiveMatch);
          return { ...obj, offerLive: value };
        }
        return obj;
      });
    },
    setIsEdit: (state, action) => {
      state.isEdit = action.payload;
    },
    collapseRow: (state, action) => {
      const { osMatchId, ...rest } = action.payload;
      const { columnKey, mode } = rest;
      state.collapseExpandRowState.columnKey = columnKey;
      state.collapseExpandRowState.mode = mode;
      findAndUpdatePropHidenTranderMaxbetByosMatchId(
        osMatchId,
        state.entities,
        true
      );
      state.collapseExpandRowState.rowHasModeCollapse.push(osMatchId);
    },
    expandRow: (state, action) => {
      const { osMatchId, ...rest } = action.payload;
      const { columnKey, mode } = rest;
      state.collapseExpandRowState.columnKey = columnKey;
      state.collapseExpandRowState.mode = mode;
      findAndUpdatePropHidenTranderMaxbetByosMatchId(
        osMatchId,
        state.entities,
        false
      );
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line no-unused-vars
      state.collapseExpandRowState.rowHasModeCollapse =
        state.collapseExpandRowState.rowHasModeCollapse.filter(
          r => r !== osMatchId
        );
    },
    collapseAllRow: (state, action) => {
      state.collapseExpandAllRowState = action.payload;
      state.collapseExpandRowState.rowHasModeCollapse.length = 0;
      state.entities.forEach(entity => {
        updatePropHidenTranderMaxbet(0, entity.traderMaxbetModels, true);
        state.collapseExpandRowState.rowHasModeCollapse.push(entity.osMatchId);
      });
    },
    expandAllRow: (state, action) => {
      state.collapseExpandAllRowState = action.payload;
      state.entities.forEach(entity =>
        updatePropHidenTranderMaxbet(0, entity.traderMaxbetModels, false)
      );
      state.collapseExpandRowState.rowHasModeCollapse.length = 0;
    },
    order: (state, action) => {
      const sortState = action.payload;
      const { orderType, columnKey } = sortState;
      state.sortState = sortState;
      state.entities = orderBy(state.entities, columnKey, orderType);
    },
    filterBySearchKey: (state, action) => {
      const { orderType } = state.sortState;

      state.filterState = addOrReplaceFilterState(
        state.filterState,
        action.payload
      );
      state.entities = filterDataTable(
        state.entitiesOriginal,
        state.filterState,
        filterHomeOrAwayByKeyword
      );

      state.entities = orderBy(
        state.entities,
        action.payload.columnKey,
        orderType
      );
    },
    clearFilter: state => {
      const { orderType } = state.sortState;

      state.filterState = [];
      state.entities = orderBy(
        state.entitiesOriginal,
        state.sortState.columnKey,
        orderType
      );
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getOSAssignment.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getOSAssignment.fulfilled, (state, action) => {
        state.loading = REQUEST_STATUS.idle;
        state.entitiesOriginal = [...action.payload];
        state.entities = filterDataTable(
          state.entitiesOriginal,
          state.filterState,
          filterHomeOrAwayByKeyword
        );
        const { orderType, columnKey } = state.sortState;
        state.entities = orderBy(state.entities, columnKey, orderType);
        const { rowHasModeCollapse } = state.collapseExpandRowState;
        state.entities.forEach(entity => {
          const isCollapsed = rowHasModeCollapse.some(
            r => r === entity.osMatchId
          );
          isCollapsed &&
            updatePropHidenTranderMaxbet(0, entity.traderMaxbetModels, true);
        });
        const rowHasModeCollapseCloneable = [...rowHasModeCollapse];
        state.collapseExpandRowState.rowHasModeCollapse.length = 0;
        rowHasModeCollapseCloneable.forEach(matchId => {
          const rowCollapsedInNewSession = state.entities.some(
            r => r.osMatchId === matchId
          );
          rowCollapsedInNewSession &&
            state.collapseExpandRowState.rowHasModeCollapse.push(matchId);
        });
      })
      .addCase(getOSAssignment.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});

const { reducer, actions } = osLiveGameAssignmentSlice;
const {
  clearEditOfferLiveMatches,
  cancelEditOfferLive,
  upDateDataEntities,
  setIsEdit,
  order,
  orderBysport,
  filterBySearchKey,
  clearFilter,
  collapseAllRow,
  expandAllRow,
  collapseRow,
  expandRow
} = actions;
export {
  clearEditOfferLiveMatches,
  cancelEditOfferLive,
  upDateDataEntities,
  setIsEdit,
  order,
  orderBysport,
  filterBySearchKey,
  clearFilter,
  collapseAllRow,
  expandAllRow,
  collapseRow,
  expandRow
};
export default reducer;
