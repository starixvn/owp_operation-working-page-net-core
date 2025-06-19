import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';

const getUrl = (methodRoute: string) =>
  `/api/team-management/${methodRoute}`;

const getLeagues = createAsyncThunk(
  'team-management/get-info',
  async ({
    teamName,
    leagueName,
    leagueGroup
  }: {
    teamName: string | null;
    leagueName: string | null;
    leagueGroup: number;

  }) => {
    const url = getUrl('get-info');
    const response = await get({
      url,
      teamName,
      leagueName,
      leagueGroup
    });
    return response.data;
  }
);

export {
  getLeagues
};
