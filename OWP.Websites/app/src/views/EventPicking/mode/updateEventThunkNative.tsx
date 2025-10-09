import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../Shared/Service/Service';
import { updateEvents } from '../const/const.json';
import { TUpdateEvents } from '../types/dataTableType';
import { buildUrl } from '../UtilsNative';

type TUpdateEventsRequest = {
  Events: TUpdateEvents;
  EventDate: string;
};

const updateMainEventsThunk = createAsyncThunk<void, TUpdateEventsRequest>(
  'EventPicking/updateEvents',
  async ({ Events, EventDate }) => {
    const url = buildUrl(updateEvents);
    await post({ url, Events, EventDate });
  }
);


export { updateMainEventsThunk };

