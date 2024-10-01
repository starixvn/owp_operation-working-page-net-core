import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../Shared/Service/Service';
import { updateEvents } from '../const/const.json';
import { TUpdateEvents } from '../types/dataTableType';
import { buildUrl } from '../UtilsNative';

const updateMainEventsThunk = createAsyncThunk<void, TUpdateEvents>(
  'EventPicking/updateEvents',
  async events => {
    const url = buildUrl(updateEvents);
    await post({ url, Events: events });
  }
);


export { updateMainEventsThunk };

