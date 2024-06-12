import { post } from '../../../../Shared/Service/Service';
import { createAsyncThunk } from '@reduxjs/toolkit';
import {
  netCoreURL,
  getEventTicket,
  pinTicketRoute,
  deleteTicketRoute
} from '../../../const/const.json';

const genUrl = methodRoute => `${netCoreURL}/${methodRoute}`;

const getEventTickets = createAsyncThunk(
  'TicketCenter/getEventChecking',
  async modelFilter => {
    const url = genUrl(getEventTicket);
    const response = await post({ url, ...modelFilter });
    return response.data;
  }
);
const refreshEventTickets = createAsyncThunk(
  'TicketCenter/refreshEventChecking',
  async model => {
    const url = genUrl(getEventTicket);
    const response = await post({ url, ...model });
    return response.data;
  }
);

const pinTicket = createAsyncThunk('TicketCenter/pinTicket', async model => {
  const url = genUrl(pinTicketRoute);
  await post({ url, ...model });
});

const deleteTicket = createAsyncThunk(
  'TicketCenter/deleteTicket',
  async model => {
    const url = genUrl(deleteTicketRoute);
    await post({ url, ...model });
  }
);

export { getEventTickets, refreshEventTickets, pinTicket, deleteTicket };
