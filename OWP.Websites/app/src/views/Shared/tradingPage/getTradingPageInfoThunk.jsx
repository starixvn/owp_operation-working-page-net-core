import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../Service/Service';

const getSoccerTradingPageInfo = createAsyncThunk(
  'getSoccerTradingPageInfo',
  async () => {
    const url = `/api/Configuration/soccer/get`;
    const res = await get({ url });
    return res.data;
  }
);

export { getSoccerTradingPageInfo };
