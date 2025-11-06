import { createSlice, nanoid, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo, TDirectionTicketLeagueInfo } from '../../type';
import { validateTicketInfo } from './validationThunk';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    data: {
      league: '',
      event: '',
      matchId: 0,
      leagueGroup: '',
      includeTrader: false,
      ticketInfo: [{
        no: nanoid(),
        ghostNameId: undefined,
        ghostDetail: undefined,
        bettypeId: undefined,
        betOnId: undefined,
        hdp_ou: undefined,
        odds: '',
        note: '',
        remark: '',
        errorMessage: undefined,
      }],
    } as TDirectionTicket,
  },
  extraReducers: builder => {
    builder.addCase(validateTicketInfo.fulfilled, (state, action) => {
      for (const ticket of state.data.ticketInfo) {
        ticket.errorMessage = action.payload.message.find(x => x.no === ticket.no);
      }
    });
  },
  reducers: {
    setLeagueInfo: (state, action: PayloadAction<TDirectionTicketLeagueInfo>) => {
      const { league, event, matchId, leagueGroup } = action.payload;
      debugger
      state.data.league = league;
      state.data.event = event;
      state.data.matchId = matchId;
      state.data.leagueGroup = leagueGroup;
    },
    addTicketInfo: state => {
      state.data.ticketInfo.push({
        no: nanoid(),
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
        no: nanoid(),
        ghostNameId: undefined,
        ghostDetail: undefined,
        bettypeId: undefined,
        betOnId: undefined,
        hdp_ou: undefined,
        odds: '',
        note: '',
        remark: '',
        errorMessage: undefined,
      }];
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const { updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData, setLeagueInfo } = actions;
export { actions, updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData, setLeagueInfo };

export default reducer;
