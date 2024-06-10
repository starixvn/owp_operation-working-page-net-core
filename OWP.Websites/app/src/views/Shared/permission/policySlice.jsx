import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../Constants';
import { queryPolicy } from './policyThunkNative';

const policySlice = createSlice({
  name: 'permission',
  initialState: {
    permission: {},
    loading: REQUEST_STATUS.idle,
    error: null
  },
  reducers: {
    updatePolicyState: (state, action) => {
      state.permission = action.payload;
    }
  },
  extraReducers: builder=>{
    builder.addCase(queryPolicy().pending, state => {
      state.loading = REQUEST_STATUS.pending;
    })
    .addCase(queryPolicy().fulfilled, (state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.permission = action.payload;
    })
    .addCase(queryPolicy().rejected,(state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.error = action.error;
    });
  }
});

const { actions, reducer } = policySlice;
const { updatePolicyState } = actions;
export { actions, queryPolicy, updatePolicyState };

export default reducer;
