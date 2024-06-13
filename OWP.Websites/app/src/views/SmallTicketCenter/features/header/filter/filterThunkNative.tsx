import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { getTicketType } from '../../../const/const.json';
import { TTicketTypeResponse } from '../../../type';
import { netCoreURL } from '../../../services/service';

const getTicketTypeOption = createAsyncThunk<TTicketTypeResponse>(
  'group/GetRecipientGroup',
  async () => {
    const url = `${netCoreURL}/${getTicketType}`;

    const response = await get({ url });
    return response.data;
  }
);

export { getTicketTypeOption };
