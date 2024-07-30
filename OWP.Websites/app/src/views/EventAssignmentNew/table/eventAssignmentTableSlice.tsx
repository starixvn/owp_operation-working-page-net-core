import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../Shared/Constants';
import {
  TColumnFilter,
  TDataSource,
  TQueryLiveGameAssignment,
  TUpdateAssignment
} from '../type/type';
import queryDataTableThunk from './queryDataTableThunk';
import { isEqual } from 'lodash';
import {
  handleFilter,
  updateColumnFilter
} from './columnFilter';
import { buildIsReadValue } from '../../Shared/ticketEvent/helper/ticketHelper';

const eventAssignmentTableSlice = createSlice({
  name: 'eventAssignmentTableSlice',
  initialState: {
    dateUpdated: '',
    syncOffVRLastTime: '',
    syncOffVRByUser: '',
    filterState: [],
    entitiesOriginal: [],
    entities: [],
    isShowUnreadMsg: false,
    loading: REQUEST_STATUS.pending
  } as TQueryLiveGameAssignment,
  reducers: {
    onUpdatedAssignment: (state, action: PayloadAction<TUpdateAssignment>) => {
      const { assignment, bettypeName, rowIndex } = action.payload;
      if (assignment && bettypeName) {
        let assignments = state.entities[rowIndex][bettypeName];
        if (assignments) {
          const index = assignments.findIndex(
            el =>
              isEqual(el.AssignmentId, assignment.AssignmentId) &&
              isEqual(el.OWPMatchId, assignment.OWPMatchId)
          );
          if (index === -1) {
            assignments.push(assignment);
          } else {
            assignments[index] = assignment;
          }

          assignments = assignments.filter( // NOSONAR
            a => a.AssignmentId === 0 || a.AssignmentId === null
          );
        }
      }
    },
    onDeletedAssignment: (state, action: PayloadAction<TUpdateAssignment>) => {
      const { assignment, bettypeName, rowIndex } = action.payload;
      if (assignment && bettypeName) {
        const event = state.entities[rowIndex];
        if (assignment.AssignmentType === 1) {
          event[bettypeName] = [];
        } else {
          let assignments = event[bettypeName];
          assignments = assignments.filter(a => a.AssignmentId);
          event[bettypeName] = assignments;
        }
      }
    },
    onColumnFilter: (state, action: PayloadAction<TColumnFilter>) => {
      updateColumnFilter(state.filterState, action.payload);
      state.entities =
        handleFilter(state.entitiesOriginal)(state.filterState)(
          state.isShowUnreadMsg
        ) || [];
    },
    onClearFilter: state => {
      state.filterState = [];
      state.entities = state.entitiesOriginal;
    },
    handleShowUnreadMessage: state => {
      const { isShowUnreadMsg } = state;
      const currentValue = !isShowUnreadMsg;
      state.isShowUnreadMsg = currentValue;
      state.entities =
        handleFilter(state.entitiesOriginal)(state.filterState)(currentValue) ||
        [];
    },

    setDataTable: (state, action) => {
      state.entities = action.payload;
    },

    // TODO
    setDataSignalR: (state, action) => {
      const { actorId, parentMatchId, userId } = action.payload;
      state.entities.forEach((item: TDataSource) => {
        if (item.ParentMatchId === parentMatchId) {
          item.IsRead = buildIsReadValue(actorId, userId, item.IsRead);
          item.HasTicket = true;
        }
      });
    }
  },
  extraReducers: builder => {
    builder
      .addCase(queryDataTableThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(queryDataTableThunk.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        state.entities =
          handleFilter(payload.data)(state.filterState)(
            state.isShowUnreadMsg
          ) || [];
        state.entitiesOriginal = payload.data;
        state.syncOffVRLastTime = payload.syncOffVRLastUpdated.item1;
        state.syncOffVRByUser = payload.syncOffVRLastUpdated.item2;
        state.dateUpdated = payload.dateUpdated;
      })
      .addCase(queryDataTableThunk.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});
const { reducer, actions } = eventAssignmentTableSlice;
const {
  onDeletedAssignment,
  onUpdatedAssignment,
  onColumnFilter,
  onClearFilter,
  setDataTable,
  setDataSignalR,
  handleShowUnreadMessage
} = actions;

export {
  onDeletedAssignment,
  onUpdatedAssignment,
  onColumnFilter,
  onClearFilter,
  setDataTable,
  setDataSignalR,
  handleShowUnreadMessage
};
export default reducer;
