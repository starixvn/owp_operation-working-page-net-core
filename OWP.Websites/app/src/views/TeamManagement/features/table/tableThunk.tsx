import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';

const getUrl = (methodRoute: string) =>
  `/api/team-management/${methodRoute}`;

const getLeagues = createAsyncThunk(
  'team-management/get-info',
  async ({
    TeamName,
    LeagueName,
    LeagueGroupId
  }: {
    TeamName: string | null;
    LeagueName: string | null;
    LeagueGroupId: number;

  }) => {
    const url = getUrl('get-info');
    const response = await get({
      url,
      TeamName,
      LeagueName,
      LeagueGroupId
    });
    return response.data;
  }
);

export {
  getLeagues
};
