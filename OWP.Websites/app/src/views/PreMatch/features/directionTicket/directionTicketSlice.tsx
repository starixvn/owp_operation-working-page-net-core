import { createSlice, nanoid, PayloadAction } from '@reduxjs/toolkit';
import { TDirectionTicket, TRemoveTicketInfo, TUpdateTicketInfo, TDirectionTicketLeagueInfo, TUpdateBetType } from '../../type';
import { validateTicketInfo } from './validationThunk';
import { BETON_OPTIONS, BETTYPE_OPTIONS } from '../../const/const';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    data: {
      league: '',
      event: '',
      matchId: 0,
      leagueGroup: '',
      isIncludeTrader: true,
      ticketInfo: [{
        no: nanoid(),
        ghostName: undefined,
        ghostDetail: undefined,
        betType: undefined,
        betOn: undefined,
        hdpOu: undefined,
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

      state.data.league = league;
      state.data.event = event;
      state.data.matchId = matchId;
      state.data.leagueGroup = leagueGroup;
    },
    addTicketInfo: state => {
      state.data.ticketInfo.push({
        no: nanoid(),
        ghostName: undefined,
        ghostDetail: '',
        betType: undefined,
        betOn: undefined,
        hdpOu: undefined,
        odds: '',
        note: '',
        remark: '',
      });
    },
    updateIncludeTrader: (state, action: PayloadAction<boolean>) => {
      state.data.isIncludeTrader = action.payload;
    },
    updateBetType: (state, action: PayloadAction<TUpdateBetType>) => {
      const { index, value } = action.payload;
      const ticket = state.data.ticketInfo[index];
      ticket.betType = value;

      const betOnOptions = (ticket.betType === BETTYPE_OPTIONS[0].value
        || ticket.betType === BETTYPE_OPTIONS[1].value)
        ? BETON_OPTIONS[0]
        : BETON_OPTIONS[1];

      if (!betOnOptions.some(option => option.value === ticket.betOn)) {
        ticket.betOn = undefined;
      }
    },
    updateTicketInfo: (state, action: PayloadAction<TUpdateTicketInfo>) => {
      const { index, field, value } = action.payload;
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
      state.data.isIncludeTrader = true;
      state.data.ticketInfo = [{
        no: nanoid(),
        ghostName: undefined,
        ghostDetail: undefined,
        betType: undefined,
        betOn: undefined,
        hdpOu: undefined,
        odds: '',
        note: '',
        remark: '',
        errorMessage: undefined,
      }];
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const { updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData, setLeagueInfo, updateIncludeTrader, updateBetType } = actions;
export { actions, updateTicketInfo, removeTicketInfo, addTicketInfo, clearPopupData, setLeagueInfo, updateIncludeTrader, updateBetType };

export default reducer;
