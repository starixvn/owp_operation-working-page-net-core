import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../../Shared/Service/Service';
import { netCoreURLTicketCenter } from '../../../const/apiUrl';
import { TMessageDetail } from '../../../type';

const queryConsolidationThunk = createAsyncThunk<TMessageDetail[], {typeId: number, subTypeId: number|null}>(
  'consolidation/query',
  async ({typeId, subTypeId}) => {
    const url = `${netCoreURLTicketCenter}/get-message-three-day-ago?ticketTypeId=${typeId}&&subtypeId=${subTypeId}`;

    const response = await get({ url});
    return response.data;
  }
);

export {
  queryConsolidationThunk
};