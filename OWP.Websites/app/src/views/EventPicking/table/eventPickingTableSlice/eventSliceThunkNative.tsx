import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../../Shared/Service/Service';
import { getEventPicking, editEventPicking } from '../../const/const.json';
import { TResult } from '../../types/dataTableType';
import { TFilter } from '../../types/filterType';
import { buildUrl } from '../../UtilsNative';
;

const getEventPickings = createAsyncThunk<TResult, TFilter>(
  'EventPicking/getEventPicking',
  async modelFilter => {
    const url = buildUrl(getEventPicking);
    const response = await post({ url, ...modelFilter });
    return response.data as TResult;
  }
);

const editEventPickings = createAsyncThunk<TResult, TFilter>(
  'EventPicking/editEventPicking',
  async modelFilter => {
    const url = buildUrl(editEventPicking);
    const response = await post({ url, ...modelFilter });
    return response.data as TResult;
  }
);

export { getEventPickings, editEventPickings };

