import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { CONSOLIDATION_ROOT_URL, GET_SUBTYPE } from '../../../const/const.json';
import { TSubTypeRow } from '../../../type';


const querySubtypeThunk = createAsyncThunk<TSubTypeRow[]>(
  'subtype/query',
  async () => {
    const url = `${CONSOLIDATION_ROOT_URL}/${GET_SUBTYPE}`;

    const response = await get({ url});
    return response.data;
  }
);

export {
  querySubtypeThunk
};