import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { TQueryPolicy } from '../type/type';
import queryPolicyThunk from './queryPolicyThunk';

const policySlice = createSlice({
  name: 'policySlice',
  initialState: {
    entities: {},
    loading: REQUEST_STATUS.pending
  } as TQueryPolicy,
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(queryPolicyThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(queryPolicyThunk.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        state.entities = payload;
      })
      .addCase(queryPolicyThunk.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});
const { reducer } = policySlice;

export default reducer;
