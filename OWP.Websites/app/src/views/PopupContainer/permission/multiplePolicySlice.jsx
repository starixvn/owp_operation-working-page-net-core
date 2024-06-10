import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { queryPolicy } from './policyThunkNative';

const createMultiplePolicySlice = (sliceName = 'policy') =>
  createSlice({
    name: sliceName,
    initialState: {
      permission: {},
      loading: REQUEST_STATUS.idle,
      error: null
    },
    extraReducers: builder=>{
      builder.addCase([queryPolicy(sliceName).pending],state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(queryPolicy(sliceName).fulfilled,(state, action) => {
        state.permission = action.payload;
        state.loading = REQUEST_STATUS.idle;
      })
      .addCase(queryPolicy(sliceName).rejected, (state, action) => {
        state.error = action.error;
        state.loading = REQUEST_STATUS.idle;
      });
    }
  });

const createReducer = name => {
  const slice = createMultiplePolicySlice(name);
  return slice.reducer;
};

export { queryPolicy };
export default createReducer;
