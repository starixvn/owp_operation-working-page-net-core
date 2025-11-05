import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo } from '../../type';
import { GHOST_OPTIONS, BETTYPE_OPTIONS } from '../../const/const';

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
        ghostNameId: GHOST_OPTIONS[0].value,
        ghostDetail: '',
        bettypeId: BETTYPE_OPTIONS[0].value,
        betOnId: '',
        hdp_ou: 0,
        odds: '',
      }],
    } as TDirectionTicket,
  },
  reducers: {
    addTicketInfo: state => {
      state.data.ticketInfo.push({
        ghostNameId: 0,
        ghostDetail: '',
        bettypeId: 0,
        betOnId: '',
        hdp_ou: 0,
        odds: '',
        note: '',
        remark: '',
      });
    },
    updateTicketInfo: (state, action: PayloadAction<TUpdateTicketInfo>) => {
      const { index, key, value } = action.payload;
      const ticket = state.data.ticketInfo[index];
      ticket[key] = value as never;
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
