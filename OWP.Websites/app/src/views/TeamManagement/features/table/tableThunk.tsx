import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';

const getUrl = (methodRoute: string) =>
  `/api/TeamManagement/${methodRoute}`;

const getLeagues = createAsyncThunk(
  'team-management/get-leagues',
  async ({
    leagueName,
    leagueGroup,
    teamName
  }: {
    leagueName: string | null;
    leagueGroup: number;
    teamName: string | null;

  }) => {
    const url = getUrl('get-team-management');
    const response = await get({
      url,
      leagueName,
      leagueGroup,
      teamName
    });
    return response.data;
  }
);

export {
  getLeagues
};
