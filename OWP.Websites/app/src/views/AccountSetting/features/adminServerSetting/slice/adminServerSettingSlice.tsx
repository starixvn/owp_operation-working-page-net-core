import { createSlice } from '@reduxjs/toolkit';
import { TServer } from '../../../Type';
import type { PayloadAction } from '@reduxjs/toolkit';
import { queryAdminServerSettingThunk, querySelectedAdminServerThunk } from './AdminServerSettingThunk';

const adminServerSettingSlice = createSlice({
  name: 'adminServerSetting',
  initialState: {
    servers: [] as TServer[],
    selectedServer: 0
  },
  reducers: {
    setServers: (state, action: PayloadAction<TServer[]>) => {
      state.servers = action.payload;
    },
    setSelectedServer: (state, action: PayloadAction<number>) => {
      state.selectedServer = action.payload;
    }
  },
  extraReducers: builder => {
    builder.addCase(queryAdminServerSettingThunk.fulfilled, (state, { payload }) => {
      state.servers = payload;
    });
    builder.addCase(querySelectedAdminServerThunk.fulfilled, (state, { payload }) => {
      state.selectedServer = payload;
    });
  }
});

const { reducer, actions } = adminServerSettingSlice;
const { setServers, setSelectedServer } = actions;
export { setServers, setSelectedServer };

export default reducer;