import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';

const getUrl = (methodRoute: string) =>
  `/api/TeamManagement/${methodRoute}`;

const getTeams = createAsyncThunk(
  'team-management/get-team',
  async ({
    leagueGroup
  }: {
    leagueGroup: string | null;
  }) => {
    const url = getUrl('get-team-management');
    const response = await get({
      url,
      leagueGroup
    });
    return response.data;
  }
);

export {
  getTeams
};
