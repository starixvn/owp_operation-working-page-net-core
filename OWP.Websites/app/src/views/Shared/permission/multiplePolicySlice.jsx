import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../Constants';
import { queryPolicy } from './policyThunkNative';

const createMultiplePolicySlice = (sliceName = 'policy') =>
  createSlice({
    name: sliceName,
    initialState: {
      permission: {
        Username: undefined,
        HasRead: false,
        HasWrite: false,
        ActorId: 0
      },
      loading: REQUEST_STATUS.idle,
      error: null
    },
    extraReducers: builder => {
      builder
        .addCase(queryPolicy(sliceName).pending, state => {
          state.loading = REQUEST_STATUS.pending;
        })
        .addCase(queryPolicy(sliceName).fulfilled, (state, action) => {
          state.loading = REQUEST_STATUS.idle;
          state.permission = action.payload;
        })
        .addCase(queryPolicy(sliceName).rejected, (state, action) => {
          state.loading = REQUEST_STATUS.idle;
          state.error = action.error;
        });
    }
  });

const createReducer = sliceName => {
  const slice = createMultiplePolicySlice(sliceName);
  return slice.reducer;
};

export { queryPolicy };
export default createReducer;
