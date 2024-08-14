import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { TNode } from '../../../../Shared/treeView/type';
import { CONSOLIDATION_ROOT_URL, GET_SUBTYPE } from '../../../const/const.json';


const querySubtypeThunk = createAsyncThunk<TNode[]>(
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