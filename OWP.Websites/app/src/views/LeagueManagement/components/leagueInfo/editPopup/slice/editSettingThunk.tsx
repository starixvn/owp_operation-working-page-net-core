import { createAsyncThunk } from '@reduxjs/toolkit';
import { buildLeagueGroupURL, buildRequestURL } from '../../../../../Shared/Service/serviceHelper';
import { get, post } from '../../../../../Shared/Service/Service';
import { TOption, TUpdateSettingSender } from '../EditType';
import { rebuildLeagueGroupOption } from '../../../../helper/commonHelper';
import { convertResourceToOptions } from '../helper/helper';

const getLeagueGroupOptionsThunk = createAsyncThunk<TOption[]>(
  'leagueGroup/query',
  async () => {
    const url = buildLeagueGroupURL();
    const data = await get({ url });

    const options = convertResourceToOptions(data.data);
    const leagueGroupOptionIncludeNoneItem = rebuildLeagueGroupOption(options);
    return leagueGroupOptionIncludeNoneItem as TOption[];
  }
);

const updateSettingService = async (model: TUpdateSettingSender) => {
  const url = buildRequestURL('NewLeagueSetting', 'editLeagueSetting');
  const response = await post({ url, ...model });
  return response;
};

export { getLeagueGroupOptionsThunk, updateSettingService };
