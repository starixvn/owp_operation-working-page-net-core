import { createAsyncThunk } from '@reduxjs/toolkit';
import { get, post } from '../../../../Shared/Service/Service';
import { TServer } from '../../../Type';


const queryAdminServerSettingThunk = createAsyncThunk<TServer[]>(
  'adminServerSetting/queryServer',
  async () => {
    const response = await get({ url: `/api/Configuration/soccer/get` });
    const { servers, tradingPage } = response.data;
    const defaultServer = {
      port: 80,
      name: 'Main',
      url: tradingPage,
      id: 0
    };
    return [defaultServer, ...(servers || [])];
  }
);

const saveAdminServerSettingThunk = createAsyncThunk<void, { value: number }>(
  'adminServerSetting/saveServer',
  async model => {
    const response = await post({ url: `/api/AccountSetting/UserSetting/update-user-setting-server`, ...model });
    return response.data;
  }
);

const querySelectedAdminServerThunk = createAsyncThunk<number, void>(
  'adminServerSetting/getServer',
  async () => {
    const response = await get({ url: `/api/AccountSetting/UserSetting/get-user-setting-server` });
    return response.data || 0;
  }
);


export { queryAdminServerSettingThunk, saveAdminServerSettingThunk, querySelectedAdminServerThunk };

