import { createSlice } from '@reduxjs/toolkit';
import { getSoccerTradingPageInfo } from './getTradingPageInfoThunk';
import { REQUEST_STATUS } from '../Constants';

const tradingPageInfoSlice = createSlice({
  name: 'tradingPageInfoSlice',
  initialState: {
    info: {}
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
      });
  }
});

const { reducer } = tradingPageInfoSlice;

export default reducer;
