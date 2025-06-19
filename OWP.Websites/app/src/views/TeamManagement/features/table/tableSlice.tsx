import { createSlice } from '@reduxjs/toolkit';
import { getLeagues } from './tableThunk';
import { TLeague } from '../../type/table';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import type { PayloadAction } from '@reduxjs/toolkit';

const tableSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    loadingDataTable: 'first-time',
    leagues: [] as TLeague[]
  },
  reducers: {
    setValue: (state, action: PayloadAction<{ leagueId: number; key: string; value: any; }>) => {
      const { leagueId, key, value } = action.payload;
      const league = state.leagues.find(d => d.LeagueId === leagueId);
      league && ((league as any)[key] = value);
    },

    setDataTable: (state, action) => {
      state.leagues = action.payload;
    },

    toggleLeagueExpansion: (state, action: PayloadAction<number>) => {
      const leagueId = action.payload;
      const leagueGroup = state.leagues.find(lg => lg.LeagueId === leagueId);
      if (leagueGroup) {
        leagueGroup.IsExpanded = !leagueGroup.IsExpanded;
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getLeagues.pending, state => {
        state.loadingDataTable = REQUEST_STATUS.pending;
      })
      .addCase(
        getLeagues.fulfilled,
        (state, { payload }) => {
          state.loadingDataTable = REQUEST_STATUS.idle;
          state.leagues = payload;
        }
      )
      .addCase(getLeagues.rejected, state => {
        state.loadingDataTable = REQUEST_STATUS.idle;
      });
  }
});

const { actions, reducer } = tableSlice;
const {
  setValue,
  setDataTable,
  toggleLeagueExpansion
} = actions;
export {
  setValue,
  setDataTable,
  toggleLeagueExpansion
};
export default reducer;
