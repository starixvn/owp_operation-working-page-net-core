import { createAsyncThunk } from '@reduxjs/toolkit';
import { buildRequestURL } from '../../../../../Shared/Service/serviceHelper';
import { get } from '../../../../../Shared/Service/Service';
import { TDataSource } from '../TableType';

const getLeagueInfoThunk = createAsyncThunk<TDataSource[], { league: string, isEqualLeague: boolean }>(
  'dataTable/query',
  async ({ league = '', isEqualLeague = false }) => {
    const url = buildRequestURL('NewLeagueSetting', 'getAllListSettings');
    const data = await get({ url, league, isEqualLeague });
    return data.data as TDataSource[];
  }
);

const fetchLeagueSettingInfo = (league: string, isEqualLeague: boolean = false) => {
  return getLeagueInfoThunk({ league, isEqualLeague });
};

export { getLeagueInfoThunk, fetchLeagueSettingInfo };
