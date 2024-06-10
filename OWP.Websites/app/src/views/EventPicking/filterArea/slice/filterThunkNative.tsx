import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';
import { getLeague, getUpdateType } from '../../const/const.json';
import { buildUrl } from '../../helper/filterHelper';
import { Option } from '../../../Shared/MultipleSelection/type';

const getLeagues = createAsyncThunk<Option[], string>(
  'EventPicking/getLeague',
  async date => {
    const url = buildUrl(getLeague);
    const response = await get({ url, date });
    return response.data as Option[];
  }
);

const getUpdateTypes = createAsyncThunk<Option[]>(
  'EventPicking/getUpdateTypes',
  async _ => {
    const url = buildUrl(getUpdateType);
    const response = await get({ url });
    return response.data as Option[];
  }
);

export { getLeagues, getUpdateTypes };

