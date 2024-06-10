import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { getTicketTypeRecipients, saveTicketTypeRecipients } from './ticketTypeThunkNative';
import { TicketTypeRecipientDataSource } from './TypeNative';

const ticketTypeRecipientSlice = createSlice({
  name: 'ticketTypeRecipientSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    entities: [] as TicketTypeRecipientDataSource[]
  },
  reducers: {
  },
  extraReducers: builder => {
    builder.addCase(getTicketTypeRecipients.pending, state => {
      state.loading = REQUEST_STATUS.pending;
    });
    builder.addCase(getTicketTypeRecipients.fulfilled, (state, { payload }) => {
      state.loading = REQUEST_STATUS.idle;
      state.entities = payload;
    });
    builder.addCase(getTicketTypeRecipients.rejected, state => {
      state.loading = REQUEST_STATUS.idle;
    });
    builder.addCase(saveTicketTypeRecipients.pending, state => {
      state.loading = REQUEST_STATUS.pending;
    });
    builder.addCase(saveTicketTypeRecipients.fulfilled, state => {
      state.loading = REQUEST_STATUS.idle;
    });
    builder.addCase(saveTicketTypeRecipients.rejected, state => {
      state.loading = REQUEST_STATUS.idle;
    });
  }
});

const { reducer } = ticketTypeRecipientSlice;

export { getTicketTypeRecipients };

export default reducer;
