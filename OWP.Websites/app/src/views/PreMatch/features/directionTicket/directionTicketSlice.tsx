import { createSlice, nanoid, PayloadAction } from '@reduxjs/toolkit';
import {
  TDirectionTicket,
  TRemoveTicketInfo,
  TUpdateTicketInfo,
  TDirectionTicketLeagueInfo,
  TUpdateBetType
} from '../../type';

type TCloneTicketInfo = {
  index: number;
};
import { validateTicketInfo } from './validationThunk';
import { getBetOnOptions } from '../../helper/preMatchHelper';

const directionTicketSlice = createSlice({
  name: 'directionTicket',
  initialState: {
    data: {
      league: '',
      event: '',
      matchId: 0,
      leagueGroup: '',
      isIncludeTrader: true,
      ticketInfo: [
        {
          no: nanoid(),
          ghostName: undefined,
          ghostDetail: undefined,
          betType: undefined,
          betOn: undefined,
          hdpOu: undefined,
          odds: '',
          note: '',
          remark: '',
          errorMessage: undefined
        }
      ]
    } as TDirectionTicket
  },
  extraReducers: builder => {
    builder.addCase(validateTicketInfo.fulfilled, (state, action) => {
      for (const ticket of state.data.ticketInfo) {
        ticket.errorMessage = action.payload.message.find(
          x => x.no === ticket.no
        );
      }
    });
  },
  reducers: {
    setLeagueInfo: (
      state,
      action: PayloadAction<TDirectionTicketLeagueInfo>
    ) => {
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
        remark: ''
      });
    },
    updateIncludeTrader: (state, action: PayloadAction<boolean>) => {
      state.data.isIncludeTrader = action.payload;
    },
    updateBetType: (state, action: PayloadAction<TUpdateBetType>) => {
      const { index, value } = action.payload;
      const ticket = state.data.ticketInfo[index];
      ticket.betType = value;

      const betOnOptions = getBetOnOptions(ticket.betType);

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
    cloneTicketInfo: (state, action: PayloadAction<TCloneTicketInfo>) => {
      const { index } = action.payload;
      const ticketToClone = state.data.ticketInfo[index];

      const clonedTicket = {
        ...ticketToClone,
        no: nanoid(),
        errorMessage: undefined
      };

      state.data.ticketInfo.splice(index + 1, 0, clonedTicket);
    },
    clearPopupData: state => {
      state.data.isIncludeTrader = true;
      state.data.ticketInfo = [
        {
          no: nanoid(),
          ghostName: undefined,
          ghostDetail: undefined,
          betType: undefined,
          betOn: undefined,
          hdpOu: undefined,
          odds: '',
          note: '',
          remark: '',
          errorMessage: undefined
        }
      ];
    }
  }
});

const { actions, reducer } = directionTicketSlice;
const {
  updateTicketInfo,
  removeTicketInfo,
  addTicketInfo,
  clearPopupData,
  setLeagueInfo,
  updateIncludeTrader,
  updateBetType,
  cloneTicketInfo
} = actions;
export {
  actions,
  updateTicketInfo,
  removeTicketInfo,
  addTicketInfo,
  clearPopupData,
  setLeagueInfo,
  updateIncludeTrader,
  updateBetType,
  cloneTicketInfo
};

export default reducer;
