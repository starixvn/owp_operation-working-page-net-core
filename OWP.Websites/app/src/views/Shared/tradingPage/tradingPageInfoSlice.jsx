import { createSlice } from '@reduxjs/toolkit';
import {
  getSoccerTradingPageInfo,
  getUserSettingServer
} from './getTradingPageInfoThunk';
import { REQUEST_STATUS } from '../Constants';

const tradingPageInfoSlice = createSlice({
  name: 'tradingPageInfoSlice',
  initialState: {
    info: {},
    serverSetingId: 0
  },
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(getSoccerTradingPageInfo.pending, state => {
        state.queryTradingPage = REQUEST_STATUS.pending;
      })
      .addCase(getSoccerTradingPageInfo.fulfilled, (state, { payload }) => {
        state.info = payload;
        state.queryTradingPage = REQUEST_STATUS.idle;
      })
      .addCase(getUserSettingServer.fulfilled, (state, { payload }) => {
        state.serverSetingId = payload;
      });
  }
});

const { reducer } = tradingPageInfoSlice;

export default reducer;
