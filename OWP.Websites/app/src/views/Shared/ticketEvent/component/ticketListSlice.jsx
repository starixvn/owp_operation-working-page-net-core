import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { basePath } from '../helper/ticketHelper';
import { URL } from '../ticket.json';

const getTicketList = createAsyncThunk('get-ticket-list', async model => {
  const API_BASE_URL = `${window.location.origin}${basePath}${URL.GET_TICKET_LIST}`;
  const response = await axios.get(API_BASE_URL, {
    params: model
  });

  return response.data;
});

const ticketListSlice = createSlice({
  name: 'chatBoard',
  initialState: {
    loading: false,
    ticketList: []
  },
  reducers: {
    setTicketList: (state, action) => {
      state.ticketList = action.payload;
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getTicketList.pending, state => {
        state.loading = true;
      })
      .addCase(getTicketList.fulfilled, (state, action) => {
        state.ticketList = action.payload;
        state.loading = false;
      });
  }
});

const { actions, reducer } = ticketListSlice;

const { setTicketList } = actions;

export { actions, getTicketList, setTicketList };

export default reducer;
