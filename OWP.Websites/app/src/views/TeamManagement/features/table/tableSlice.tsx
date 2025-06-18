import { createSlice } from '@reduxjs/toolkit';
import { getTeams } from './tableThunk';
import { TDataSource } from '../../type/table';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import type { PayloadAction } from '@reduxjs/toolkit';

// New type for hierarchical data
type TLeagueGroup = {
  leagueId: number;
  league: string;
  leagueGroup: string;
  leagueGroupId: number;
  isExpanded: boolean;
  teams: TDataSource[];
};

const tableSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    loadingDataTable: REQUEST_STATUS.idle,
    dataSource: [] as TDataSource[],
    leagueGroups: [
      {
        leagueId: 1,
        league: 'Premier League',
        leagueGroup: 'England',
        leagueGroupId: 1,
        isExpanded: false,
        teams: [
          {
            no: '1',
            leagueId: 1,
            league: 'Premier League',
            teamId: 101,
            team: 'Manchester United',
            leagueGroup: 'England',
            leagueGroupId: 1
          },
          {
            no: '2',
            leagueId: 1,
            league: 'Premier League',
            teamId: 102,
            team: 'Liverpool',
            leagueGroup: 'England',
            leagueGroupId: 1
          },
          {
            no: '3',
            leagueId: 1,
            league: 'Premier League',
            teamId: 103,
            team: 'Arsenal',
            leagueGroup: 'England',
            leagueGroupId: 1
          },
          {
            no: '4',
            leagueId: 1,
            league: 'Premier League',
            teamId: 104,
            team: 'Chelsea',
            leagueGroup: 'England',
            leagueGroupId: 1
          },
          {
            no: '5',
            leagueId: 1,
            league: 'Premier League',
            teamId: 105,
            team: 'Manchester City',
            leagueGroup: 'England',
            leagueGroupId: 1
          }
        ]
      },
      {
        leagueId: 2,
        league: 'La Liga',
        leagueGroup: 'Spain',
        leagueGroupId: 2,
        isExpanded: false,
        teams: [
          {
            no: '6',
            leagueId: 2,
            league: 'La Liga',
            teamId: 201,
            team: 'Real Madrid',
            leagueGroup: 'Spain',
            leagueGroupId: 2
          },
          {
            no: '7',
            leagueId: 2,
            league: 'La Liga',
            teamId: 202,
            team: 'Barcelona',
            leagueGroup: 'Spain',
            leagueGroupId: 2
          },
          {
            no: '8',
            leagueId: 2,
            league: 'La Liga',
            teamId: 203,
            team: 'Atletico Madrid',
            leagueGroup: 'Spain',
            leagueGroupId: 2
          },
          {
            no: '9',
            leagueId: 2,
            league: 'La Liga',
            teamId: 204,
            team: 'Sevilla',
            leagueGroup: 'Spain',
            leagueGroupId: 2
          }
        ]
      },
      {
        leagueId: 3,
        league: 'Bundesliga',
        leagueGroup: 'Germany',
        leagueGroupId: 3,
        isExpanded: false,
        teams: [
          {
            no: '10',
            leagueId: 3,
            league: 'Bundesliga',
            teamId: 301,
            team: 'Bayern Munich',
            leagueGroup: 'Germany',
            leagueGroupId: 3
          },
          {
            no: '11',
            leagueId: 3,
            league: 'Bundesliga',
            teamId: 302,
            team: 'Borussia Dortmund',
            leagueGroup: 'Germany',
            leagueGroupId: 3
          },
          {
            no: '12',
            leagueId: 3,
            league: 'Bundesliga',
            teamId: 303,
            team: 'RB Leipzig',
            leagueGroup: 'Germany',
            leagueGroupId: 3
          },
          {
            no: '13',
            leagueId: 3,
            league: 'Bundesliga',
            teamId: 304,
            team: 'Bayer Leverkusen',
            leagueGroup: 'Germany',
            leagueGroupId: 3
          }
        ]
      },
      {
        leagueId: 4,
        league: 'Serie A',
        leagueGroup: 'Italy',
        leagueGroupId: 4,
        isExpanded: false,
        teams: [
          {
            no: '14',
            leagueId: 4,
            league: 'Serie A',
            teamId: 401,
            team: 'Juventus',
            leagueGroup: 'Italy',
            leagueGroupId: 4
          },
          {
            no: '15',
            leagueId: 4,
            league: 'Serie A',
            teamId: 402,
            team: 'AC Milan',
            leagueGroup: 'Italy',
            leagueGroupId: 4
          },
          {
            no: '16',
            leagueId: 4,
            league: 'Serie A',
            teamId: 403,
            team: 'Inter Milan',
            leagueGroup: 'Italy',
            leagueGroupId: 4
          },
          {
            no: '17',
            leagueId: 4,
            league: 'Serie A',
            teamId: 404,
            team: 'Napoli',
            leagueGroup: 'Italy',
            leagueGroupId: 4
          },
          {
            no: '18',
            leagueId: 4,
            league: 'Serie A',
            teamId: 405,
            team: 'Roma',
            leagueGroup: 'Italy',
            leagueGroupId: 4
          }
        ]
      },
      {
        leagueId: 5,
        league: 'Ligue 1',
        leagueGroup: 'France',
        leagueGroupId: 5,
        isExpanded: false,
        teams: [
          {
            no: '19',
            leagueId: 5,
            league: 'Ligue 1',
            teamId: 501,
            team: 'Paris Saint-Germain',
            leagueGroup: 'France',
            leagueGroupId: 5
          },
          {
            no: '20',
            leagueId: 5,
            league: 'Ligue 1',
            teamId: 502,
            team: 'Marseille',
            leagueGroup: 'France',
            leagueGroupId: 5
          },
          {
            no: '21',
            leagueId: 5,
            league: 'Ligue 1',
            teamId: 503,
            team: 'Lyon',
            leagueGroup: 'France',
            leagueGroupId: 5
          },
          {
            no: '22',
            leagueId: 5,
            league: 'Ligue 1',
            teamId: 504,
            team: 'Monaco',
            leagueGroup: 'France',
            leagueGroupId: 5
          }
        ]
      }
    ] as TLeagueGroup[]
  },
  reducers: {
    setValue: (state, action: PayloadAction<{ leagueId: number; key: string; value: any; }>) => {
      const { leagueId, key, value } = action.payload;
      const league = state.dataSource.find(d => d.leagueId === leagueId);
      league && ((league as any)[key] = value);
    },

    setDataTable: (state, action) => {
      state.dataSource = action.payload;
    },

    toggleLeagueExpansion: (state, action: PayloadAction<number>) => {
      const leagueId = action.payload;
      const leagueGroup = state.leagueGroups.find(lg => lg.leagueId === leagueId);
      if (leagueGroup) {
        leagueGroup.isExpanded = !leagueGroup.isExpanded;
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getTeams.pending, state => {
        state.loadingDataTable = REQUEST_STATUS.pending;
      })
      .addCase(
        getTeams.fulfilled,
        (state, { payload }) => {
          state.loadingDataTable = REQUEST_STATUS.idle;
          state.dataSource = payload;
        }
      )
      .addCase(getTeams.rejected, state => {
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
