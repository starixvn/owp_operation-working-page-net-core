import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo } from '../../type';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    data: {
      league: '',
      event: '',
      matchId: 0,
      leagueGroupId: 0,
      includeTrader: false,
      ticketInfo: [{
        ghostNameId: null,
        ghostDetail: '',
        bettypeId: null,
        betOnId: '',
        hdp_ou: 0,
        odds: '',
      }],
    } as TDirectionTicket,
  },
  reducers: {
    addTicketInfo: state => {
      state.data.ticketInfo.push({
        ghostNameId: null,
        ghostDetail: '',
        bettypeId: null,
        betOnId: null,
        hdp_ou: '',
        odds: '',
        note: '',
        remark: '',
      });
    },
    updateTicketInfo: (state, action: PayloadAction<TUpdateTicketInfo>) => {
      const { index, field, value } = action.payload;
      ;
      const ticket = state.data.ticketInfo[index];
      ticket[field] = value as never;
    },

    removeTicketInfo: (state, action: PayloadAction<TRemoveTicketInfo>) => {
      state.data.ticketInfo = [
        ...state.data.ticketInfo.slice(0, action.payload.index),
        ...state.data.ticketInfo.slice(action.payload.index + 1)
      ];
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const { updateTicketInfo, removeTicketInfo, addTicketInfo } = actions;
export { actions, updateTicketInfo, removeTicketInfo, addTicketInfo };

export default reducer;
