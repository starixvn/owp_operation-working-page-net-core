import { createAsyncThunk } from '@reduxjs/toolkit';
import { buildRequestURL } from '../../../../../Shared/Service/serviceHelper';
import { post } from '../../../../../Shared/Service/Service';
import { TDataSource, TTableSlice} from '../TableType';

const getLeagueInfoThunk = createAsyncThunk<TDataSource[], { param: TTableSlice["leagueFilterHeader"], isEqualLeague: boolean }>(
  'dataTable/query',
  async ({ param, isEqualLeague = false }) => {
    const url = buildRequestURL('NewLeagueSetting', 'getAllListSettings');
    const data = await post({ url, ...param, isEqualLeague });

    return data.data as TDataSource[];
  }
);

const fetchLeagueSettingInfo = (param: TTableSlice["leagueFilterHeader"], isEqualLeague: boolean) => {
  return getLeagueInfoThunk({ param, isEqualLeague });
};

export { getLeagueInfoThunk, fetchLeagueSettingInfo };
