import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import ToastrWrapper from '../../components/ToastrWrapper';
import {
  getTicketTypeRecipientsRoute,
  saveTicketTypeRecipientsRoute
} from '../Const.json';
import { TicketTypeRecipientDataSource } from './TypeNative';
import { netCoreURLTicketTypeRecipient } from '../apiUrl';
const okStatusCode = 200;

const getTicketTypeRecipients = createAsyncThunk<
  TicketTypeRecipientDataSource[]
>('getTicketTypeRecipients', async () => {
  const url = `${netCoreURLTicketTypeRecipient}/${getTicketTypeRecipientsRoute}`;

  const response = await axios.get(url);
  return response.data;
});

const saveTicketTypeRecipients = createAsyncThunk(
  'saveTicketTypeRecipients',
  async (postModel: object, { dispatch }) => {
    const url = `${netCoreURLTicketTypeRecipient}/${saveTicketTypeRecipientsRoute}`;

    const response = await axios.post(url, postModel);

    if (response.status === okStatusCode) {
      ToastrWrapper.showInfo('Recipients have been updated successfully!');
      dispatch(getTicketTypeRecipients());
    }
  }
);

export { getTicketTypeRecipients, saveTicketTypeRecipients };
