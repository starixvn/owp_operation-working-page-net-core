import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../Shared/Service/Service';
import { updateEvents, updateSpecialProduct } from '../const/const.json';
import { TUpdateEvents, TUpdateSpecialProduct } from '../types/dataTableType';
import { buildUrl } from '../UtilsNative';

const updateMainEventsThunk = createAsyncThunk<void, TUpdateEvents>(
  'EventPicking/updateEvents',
  async events => {
    const url = buildUrl(updateEvents);
    await post({ url, Events: events });
  }
);

const updateSpecialProductThunk = createAsyncThunk<void, TUpdateSpecialProduct>(
  'EventPicking/updateSpecialProduct',
  async events => {
    const url = buildUrl(updateSpecialProduct);
    await post({ url, Events: events });
  }
);

export { updateMainEventsThunk, updateSpecialProductThunk };

