import { createSlice } from '@reduxjs/toolkit';
import { getLeagues } from './tableThunk';
import { TLeague } from '../../type/table';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import type { PayloadAction } from '@reduxjs/toolkit';

const tableSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    loadingDataTable: REQUEST_STATUS.idle,
    leagues: [] as TLeague[]
    // leagues: [
    //   {
    //     leagueId: 1,
    //     leagueName: 'Premier League',
    //     leagueGroupName: 'England',
    //     leagueGroupId: 1,
    //     isExpanded: false,
    //     teams: [
    //       { teamId: 101, teamName: 'Manchester United' },
    //       { teamId: 102, teamName: 'Liverpool' },
    //       { teamId: 103, teamName: 'Arsenal' },
    //       { teamId: 104, teamName: 'Chelsea' },
    //       { teamId: 105, teamName: 'Manchester City' }
    //     ]
    //   },
    //   {
    //     leagueId: 2,
    //     leagueName: 'La Liga',
    //     leagueGroupName: 'Spain',
    //     leagueGroupId: 2,
    //     isExpanded: false,
    //     teams: [
    //       { teamId: 201, teamName: 'Real Madrid' },
    //       { teamId: 202, teamName: 'Barcelona' },
    //       { teamId: 203, teamName: 'Atletico Madrid' },
    //       { teamId: 204, teamName: 'Sevilla' }
    //     ]
    //   },
    //   {
    //     leagueId: 3,
    //     leagueName: 'Bundesliga',
    //     leagueGroupName: 'Germany',
    //     leagueGroupId: 3,
    //     isExpanded: false,
    //     teams: [
    //       { teamId: 301, teamName: 'Bayern Munich' },
    //       { teamId: 302, teamName: 'Borussia Dortmund' },
    //       { teamId: 303, teamName: 'RB Leipzig' },
    //       { teamId: 304, teamName: 'Bayer Leverkusen' }
    //     ]
    //   },
    //   {
    //     leagueId: 4,
    //     leagueName: 'Serie A',
    //     leagueGroupName: 'Italy',
    //     leagueGroupId: 4,
    //     isExpanded: false,
    //     teams: [
    //       { teamId: 401, teamName: 'Juventus' },
    //       { teamId: 402, teamName: 'AC Milan' },
    //       { teamId: 403, teamName: 'Inter Milan' },
    //       { teamId: 404, teamName: 'Napoli' },
    //       { teamId: 405, teamName: 'Roma' }
    //     ]
    //   },
    //   {
    //     leagueId: 5,
    //     leagueName: 'Ligue 1',
    //     leagueGroupName: 'France',
    //     leagueGroupId: 5,
    //     isExpanded: false,
    //     teams: [
    //       { teamId: 501, teamName: 'Paris Saint-Germain' },
    //       { teamId: 502, teamName: 'Marseille' },
    //       { teamId: 503, teamName: 'Lyon' },
    //       { teamId: 504, teamName: 'Monaco' }
    //     ]
    //   }
    // ] as TLeague[]
  },
  reducers: {
    setValue: (state, action: PayloadAction<{ leagueId: number; key: string; value: any; }>) => {
      const { leagueId, key, value } = action.payload;
      const league = state.leagues.find(d => d.leagueId === leagueId);
      league && ((league as any)[key] = value);
    },

    setDataTable: (state, action) => {
      state.leagues = action.payload;
    },

    toggleLeagueExpansion: (state, action: PayloadAction<number>) => {
      const leagueId = action.payload;
      const leagueGroup = state.leagues.find(lg => lg.leagueId === leagueId);
      if (leagueGroup) {
        leagueGroup.isExpanded = !leagueGroup.isExpanded;
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
