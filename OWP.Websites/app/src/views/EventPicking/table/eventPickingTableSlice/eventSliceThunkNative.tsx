import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../../Shared/Service/Service';
import { getEventPicking, editEventPicking, getTurnoverMargin  } from '../../const/const.json';
import { TResult } from '../../types/dataTableType';
import { TFilter, TPeriodFilter } from '../../types/filterType';
import { buildUrl } from '../../UtilsNative';

type TTurnoverFilter = {
  Matches: Array<{
    InfoSiteMatchId: number;
    LeagueName: string;
  }>
  Period: TPeriodFilter;
};

const getEventPickings = createAsyncThunk<TResult, TFilter>(
  'EventPicking/getEventPicking',
  async (modelFilter, thunkAPI) => {
    const url = buildUrl(getEventPicking);
    const response = await post({ url, ...modelFilter });
    const result = response.data as TResult;

    return result;
  }
);

const editEventPickings = createAsyncThunk<TResult, TFilter>(
  'EventPicking/editEventPicking',
  async (modelFilter, thunkAPI) => {
    const url = buildUrl(editEventPicking);
    const response = await post({ url, ...modelFilter });
    const result = response.data as TResult;

    return result;
  }
);

const getTurnoverMargins = createAsyncThunk<TResult, TTurnoverFilter>(
  'EventPicking/getTurnoverMargin',
  async modelFilter => {
    const url = buildUrl(getTurnoverMargin);
    const response = await post({ url, ...modelFilter });

    
    return response.data as TResult;
  }
);

export { getEventPickings, editEventPickings, getTurnoverMargins };

