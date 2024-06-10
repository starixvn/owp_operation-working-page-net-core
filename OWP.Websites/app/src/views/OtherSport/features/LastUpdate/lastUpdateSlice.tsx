import { createSlice } from '@reduxjs/toolkit';
import getLastUpdateThunkNative from './getLastUpdateThunkNative';

export type loadingType = 'idle' | 'pending' | 'succeeded' | 'failed';

export interface LastUpdateState {
  entity: {};
  loading: loadingType;
}

const initialState = {
  entity: {},
  loading: 'idle'
} as LastUpdateState;

const lastUpdateSlice = createSlice({
  name: 'lastUpdateSlice',
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(getLastUpdateThunkNative.pending, (state, _) => {
        state.loading = 'pending';
      })
      .addCase(getLastUpdateThunkNative.fulfilled, (state, action) => {
        state.entity = action.payload;
        state.loading = 'idle';
      });
  }
});

export default lastUpdateSlice.reducer;
