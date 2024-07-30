/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import compareValues from '../../../../Shared/Order/compareValues';
import { isEqual } from 'lodash';
import {
  getEventTickets,
  refreshEventTickets,
  pinTicket,
  deleteTicket
} from './eventSliceThunkNative';
import {
  filterIssueByUnreadMessageValue,
  sortTickets,
  removeTickets,
  updateTicketsStatus
} from './eventSliceHelper';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { checkAllFilterCondition } from '../../signalR/signalRHelper';

const handlePendingState = state => {
  const { loading } = state;
  if (isEqual(loading, REQUEST_STATUS.idle)) {
    state.loading = REQUEST_STATUS.pending;
  }
};

const handleRejectState = (state, error) => {
  if (isEqual(state.loading, REQUEST_STATUS.pending)) {
    state.loading = REQUEST_STATUS.idle;
    state.error = error;
  }
};

const eventCheckingSlice = createSlice({
  name: 'eventChecking',
  initialState: {
    entities: [],
    entitiesOrigin: [],
    currentRequestId: undefined, // NOSONAR
    loading: REQUEST_STATUS.idle,
    error: null,
    sortState: {},
    isShowUnreadMessOnly: false,
    ticketType: [{ value: 0, label: 'All' }]
  },
  reducers: {
    updateSubTypeByTicketId: {
      reducer: (state, action) => {
        const { ticketId, ticketSubTypeId, ticketSubTypeName } = action.payload;
        state.entitiesOrigin.forEach(x => {
          if (x.ticketId === ticketId) {
            x.ticketSubTypeId = ticketSubTypeId;
            x.ticketSubTypeName = ticketSubTypeName;
          }
        });

        state.entities.forEach(x => {
          if (x.ticketId === ticketId) {
            x.ticketSubTypeId = ticketSubTypeId;
            x.ticketSubTypeName = ticketSubTypeName;
          }
        });
        sortTickets(state.entities);
        sortTickets(state.entitiesOrigin);
      }
    },
    appendTicketItemIntoDataSource: {
      reducer: (state, action) => {
        state.entities.unshift(action.payload);
        state.entitiesOrigin.unshift(action.payload);
        sortTickets(state.entitiesOrigin);
        state.entities = filterIssueByUnreadMessageValue(state);
      }
    },
    updateTicketItemIntoDataSource: {
      reducer: (state, action) => {
        const { ticketInfo, filterModel } = action.payload;
        const ticketId = ticketInfo?.ticketId;
        const isUpdate = checkAllFilterCondition(filterModel, ticketInfo);
        if (isUpdate) {
          const ticketIndex = state.entitiesOrigin.findIndex(
            e => e.ticketId === ticketId
          );
          if (ticketIndex !== -1) {
            state.entitiesOrigin[ticketIndex] = ticketInfo;
          } else {
            state.entitiesOrigin.unshift(ticketInfo);
          }
        } else {
          state.entitiesOrigin = removeTickets(state.entitiesOrigin, ticketId);
        }
        sortTickets(state.entitiesOrigin);
        state.entities = filterIssueByUnreadMessageValue(state);
      }
    },
    removeTicketItemFromDataSource: {
      reducer: (state, action) => {
        const ticketInfo = action.payload;
        const { ticketId } = ticketInfo;
        state.entities = state.entities.filter(
          item => item.ticketId !== ticketId
        );
        state.entitiesOrigin = state.entitiesOrigin.filter(
          item => item.ticketId !== ticketId
        );
      }
    },
    sort: {
      reducer: (state, action) => {
        const { columnKey, orderType } = action.payload;
        state.sortState = { columnKey, orderType };
        const type = isEqual(orderType, 'ORDER_ASC') ? 'asc' : 'desc';
        const pinnedEntities = state.entities.filter(x => x.isPinned);
        const unpinEntities = state.entities.filter(x => !x.isPinned);
        unpinEntities.sort(compareValues(columnKey, type));
        state.entities = pinnedEntities.concat(unpinEntities);
      }
    },
    showUnreadMessageOnly: {
      reducer: (state, action) => {
        state.isShowUnreadMessOnly = action.payload;
        state.entities = filterIssueByUnreadMessageValue(state);
      }
    },
    updatedIsReadOneRow: {
      reducer: (state, action) => {
        const rowIndex = action.payload;
        state.entities[rowIndex].isRead = true;
        state.entitiesOrigin[rowIndex].isRead = true;
      }
    },
    updatedIsReadAllRows: {
      reducer: state => {
        state.entitiesOrigin.forEach(x => {
          x.isRead = true;
        });
        state.entities = filterIssueByUnreadMessageValue(state);
      }
    },
    setTicketStatus: (state, { payload: data }) => {
      const { ticketInfo, filterModel } = data;
      const isCorrectFilter = checkAllFilterCondition(filterModel, ticketInfo);
      if (isCorrectFilter) {
        updateTicketsStatus(state.entities, ticketInfo);
        updateTicketsStatus(state.entitiesOrigin, ticketInfo);
        sortTickets(state.entities);
        sortTickets(state.entitiesOrigin);
      } else {
        state.entities = removeTickets(state.entities, ticketInfo.ticketId);
        state.entitiesOrigin = removeTickets(
          state.entitiesOrigin,
          ticketInfo.ticketId
        );
      }
    },
    setNewMessage: (state, action) => {
      const {
        ticketId,
        createdDate,
        fileId,
        isFile,
        message,
        messageId,
        userId,
        username,
        isRead
      } = action.payload;
      const newMsg = {
        ticketId,
        createdDate,
        fileId,
        isFile,
        message,
        messageId,
        userId,
        username
      };
      const ticketTarget = state.entities.find(e => e.ticketId === ticketId);

      ticketTarget.isRead = isRead;
      ticketTarget.messages.unshift(newMsg);
      if (ticketTarget.messages?.length > 2) {
        ticketTarget.messages.pop();
      }
      const ticketOrigin = state.entitiesOrigin.find(
        e => e.ticketId === ticketId
      );
      ticketOrigin.isRead = isRead;
      ticketOrigin.messages.unshift(newMsg);
      if (ticketOrigin.messages?.length > 2) {
        ticketOrigin.messages.pop();
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getEventTickets.pending, (state, action) => {
        const { loading } = state;
        if (isEqual(loading, REQUEST_STATUS.idle)) {
          state.currentRequestId = action.meta.requestId;
          state.loading = REQUEST_STATUS.pending;
        }
      })
      .addCase(getEventTickets.fulfilled, (state, action) => {
        const { requestId } = action.meta;
        if (
          isEqual(state.loading, REQUEST_STATUS.pending) &&
          isEqual(state.currentRequestId, requestId)
        ) {
          state.entities.length = 0;
          state.loading = REQUEST_STATUS.idle;
          state.currentRequestId = null;
          state.entitiesOrigin = [...[], ...action.payload];
          state.entities = filterIssueByUnreadMessageValue(state);
        }
      })
      .addCase(getEventTickets.rejected, (state, action) => {
        const { requestId } = action.meta;
        if (
          isEqual(state.loading, REQUEST_STATUS.pending) &&
          isEqual(state.currentRequestId, requestId)
        ) {
          state.currentRequestId = null;
          state.loading = REQUEST_STATUS.idle;
          state.error = action.error;
        }
      })
      .addCase(refreshEventTickets.fulfilled, (state, action) => {
        state.entitiesOrigin = [...[], ...action.payload];
        const filteredTickets = filterIssueByUnreadMessageValue(state);
        const pinnedEntities = filteredTickets.filter(x => x.isPinned);
        const unpinEntities = filteredTickets.filter(x => !x.isPinned);
        state.entities = pinnedEntities.concat(unpinEntities);
      })
      .addCase(pinTicket.pending, state => handlePendingState(state))
      .addCase(pinTicket.fulfilled, state => {
        state.loading = REQUEST_STATUS.idle;
      })
      .addCase(pinTicket.rejected, (state, action) =>
        handleRejectState(state, action.error)
      )
      .addCase(deleteTicket.pending, state => handlePendingState(state))
      .addCase(deleteTicket.fulfilled, state => {
        state.loading = REQUEST_STATUS.idle;
      })
      .addCase(deleteTicket.rejected, (state, action) =>
        handleRejectState(state, action.error)
      );
  }
});

const { actions, reducer } = eventCheckingSlice;
const {
  sort,
  updatedIsReadOneRow,
  showUnreadMessageOnly,
  updatedIsReadAllRows,
  setNewMessage,
  setTicketStatus,
  appendTicketItemIntoDataSource,
  removeTicketItemFromDataSource,
  updateTicketItemIntoDataSource,
  updateSubTypeByTicketId
} = actions;
export {
  actions,
  sort,
  updatedIsReadOneRow,
  showUnreadMessageOnly,
  updatedIsReadAllRows,
  setNewMessage,
  setTicketStatus,
  appendTicketItemIntoDataSource,
  removeTicketItemFromDataSource,
  updateTicketItemIntoDataSource,
  updateSubTypeByTicketId
};
export default reducer;
