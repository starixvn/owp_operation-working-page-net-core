import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';
import { getUpdateType } from '../../const/const.json';
import { buildUrl } from '../../helper/filterHelper';
import { Option } from '../../../Shared/MultipleSelection/type';

const getUpdateTypes = createAsyncThunk<Option[]>(
  'EventPicking/getUpdateTypes',
  async _ => {
    const url = buildUrl(getUpdateType);
    const response = await get({ url });
    return response.data as Option[];
  }
);

export { getUpdateTypes };

