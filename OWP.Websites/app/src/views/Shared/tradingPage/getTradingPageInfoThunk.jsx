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

const getUserSettingServer = createAsyncThunk(
  'getUserSettingServer',
  async () => {
    const url = `/api/AccountSetting/UserSetting/get-user-setting-server`;
    const res = await get({ url });
    return res.data || 0;
  }
);

export { getSoccerTradingPageInfo, getUserSettingServer };
