import { rebuildLeagueGroupOption } from '../../../helper/commonHelper';
import { convertResourceToOptions } from '../helper/helper';
import { createAsyncThunk } from '@reduxjs/toolkit';
import { buildLeagueGroupURL } from '../../../../Shared/Service/serviceHelper';
import { get } from '../../../../Shared/Service/Service';
import { TOption } from './Type';

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
export { getLeagueGroupOptionsThunk };
