import { createSlice, nanoid, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo } from '../../type';
import { GHOST_OPTIONS, BETTYPE_OPTIONS } from '../../const/const';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    directionTicket: {
      league: '',
      event: '',
      matchId: 0,
      leagueGroupId: 0,
      includeTrader: false,
      ticketInfo: [{
        no: nanoid(),
        ghostNameId: GHOST_OPTIONS[0].value,
        ghostDetail: '',
        bettypeId: BETTYPE_OPTIONS[0].value,
        betOnId: 0,
        hdp_ou: 0,
        odds: 0,
      }],
    } as TDirectionTicket,
  },
  reducers: {
    addTicketInfo: state => {
      state.directionTicket.ticketInfo.push({
        no: nanoid(),
        ghostNameId: 0,
        ghostDetail: '',
        bettypeId: 0,
        betOnId: 0,
        hdp_ou: 0,
        odds: 0,
        note: '',
        remark: '',
      });
    },
    updateTicketInfo: (state, action: PayloadAction<TUpdateTicketInfo>) => {
      const { index, ticketInfo } = action.payload;
      const { ghostNameId, ghostDetail, bettypeId, betOnId, hdp_ou, odds, note, remark } = ticketInfo;
      const ticket = state.directionTicket.ticketInfo[index];
      ticket.ghostNameId = ghostNameId;
      ticket.ghostDetail = ghostDetail;
      ticket.bettypeId = bettypeId;
      ticket.betOnId = betOnId;
      ticket.hdp_ou = hdp_ou;
      ticket.odds = odds;
      ticket.note = note;
      ticket.remark = remark;
    },

    removeTicketInfo: (state, action: PayloadAction<TRemoveTicketInfo>) => {
      state.directionTicket.ticketInfo = state.directionTicket.ticketInfo
        .filter(ticket => ticket.no !== action.payload.no);
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const { updateTicketInfo, removeTicketInfo, addTicketInfo } = actions;
export { actions, updateTicketInfo, removeTicketInfo, addTicketInfo };

export default reducer;
