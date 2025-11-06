import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo } from '../../type';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    data: {
      league: '',
      event: '',
      matchId: undefined,
      leagueGroupId: undefined,
      includeTrader: false,
      ticketInfo: [{
        ghostNameId: undefined,
        ghostDetail: undefined,
        bettypeId: undefined,
        betOnId: undefined,
        hdp_ou: undefined,
        odds: '',
        note: '',
        remark: '',
      }],
    } as TDirectionTicket,
  },
  reducers: {
    addTicketInfo: state => {
      state.data.ticketInfo.push({
        ghostNameId: undefined,
        ghostDetail: '',
        bettypeId: undefined,
        betOnId: undefined,
        hdp_ou: undefined,
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
    },
    clearPopupData: state => {
      state.data.ticketInfo = [{
        ghostNameId: undefined,
        ghostDetail: undefined,
        bettypeId: undefined,
        betOnId: undefined,
        hdp_ou: undefined,
        odds: '',
        note: '',
        remark: '',
      }];
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const { updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData } = actions;
export { actions, updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData };

export default reducer;
