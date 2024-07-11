import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { getEventTickets } from './ticketSliceThunkNative';
import { checkAllFilterCondition } from '../../signalR/signalRHelper';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { TTicket } from '../../../type';
import { removeTickets, sortTickets } from '../../../Helper/TicketCenterHelper';

const ticketSlice = createSlice({
  name: 'ticketSlice',
  initialState: {
    entities: [] as TTicket[],
    currentRequestId: '',
    loadTicket: REQUEST_STATUS.idle
  },
  reducers: {
    updateTicketItemIntoDataSource: {
      reducer: (state, action) => {
        const { ticketInfo, filterInfo } = action.payload;
        const ticketId = ticketInfo?.ticketId;
        const isUpdate = checkAllFilterCondition(filterInfo, ticketInfo);
        if (isUpdate) {
          const ticketIndex = state.entities.findIndex(
            e => e.ticketId === ticketId
          );
          if (ticketIndex !== -1) {
            state.entities[ticketIndex] = ticketInfo;
          } else {
            state.entities.unshift(ticketInfo);
          }
        } else {
          state.entities = removeTickets(state.entities, ticketId);
        }
        sortTickets(state.entities);
      }
    },
    appendTicketItemIntoDataSource: {
      reducer: (state, action) => {
        state.entities.unshift(action.payload);
        sortTickets(state.entities);
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getEventTickets.pending, (state, action) => {
        if (isEqual(state.loadTicket, REQUEST_STATUS.idle)) {
          state.currentRequestId = action.meta.requestId;
          state.loadTicket = REQUEST_STATUS.pending;
        }
      })
      .addCase(getEventTickets.fulfilled, (state, action) => {
        const { requestId } = action.meta;
        if (
          isEqual(state.loadTicket, REQUEST_STATUS.pending) &&
          isEqual(state.currentRequestId, requestId)
        ) {
          state.entities.length = 0;
          state.loadTicket = REQUEST_STATUS.idle;
          state.currentRequestId = '';
          state.entities = action.payload;
        }
      })
      .addCase(getEventTickets.rejected, (state, action) => {
        const { requestId } = action.meta;
        if (
          isEqual(state.loadTicket, REQUEST_STATUS.pending) &&
          isEqual(state.currentRequestId, requestId)
        ) {
          state.currentRequestId = '';
          state.loadTicket = REQUEST_STATUS.idle;
        }
      });
  }
});

const { actions, reducer } = ticketSlice;
const { appendTicketItemIntoDataSource, updateTicketItemIntoDataSource } =
  actions;
export {
  actions,
  appendTicketItemIntoDataSource,
  updateTicketItemIntoDataSource
};

export default reducer;
