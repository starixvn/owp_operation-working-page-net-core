import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { netCoreURL, netFwURL, getTicketType } from '../../../const/const.json';
import { TTicketTypeResponse } from '../../../type';

const getTicketTypeOption = createAsyncThunk<TTicketTypeResponse>(
  'group/GetRecipientGroup',
  async () => {
    const url =
      process.env.NODE_ENV === 'development'
        ? `${netCoreURL}/${getTicketType}`
        : `${netFwURL}/${getTicketType}`;

    const response = await get({ url });
    return response.data;
  }
);

export { getTicketTypeOption };
