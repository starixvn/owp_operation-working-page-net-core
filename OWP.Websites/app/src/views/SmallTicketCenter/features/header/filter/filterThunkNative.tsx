import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { getTicketType } from '../../../const/const.json';
import { TTicketTypeResponse } from '../../../type';

const getTicketTypeOption = createAsyncThunk<TTicketTypeResponse>(
  'group/GetRecipientGroup',
  async () => {
    const url = `/api/TicketCenter/${getTicketType}`;
    const response = await get({ url });
    return response.data;
  }
);

export { getTicketTypeOption };
