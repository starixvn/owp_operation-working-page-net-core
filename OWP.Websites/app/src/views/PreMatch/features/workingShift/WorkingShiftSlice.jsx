import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import workingShift from './workingShiftThunkNative';

const workingShiftSlice = createSlice({
  name: 'workingShift',
  initialState: {
    workingShiftData: null,
    loading: REQUEST_STATUS.idle,
    error: null
  },
  reducers: {},
  extraReducers: builder=>{
    builder.addCase(workingShift.pending, state => {
      const { loading } = state;
      if (isEqual(loading, REQUEST_STATUS.idle)) {
        state.loading = REQUEST_STATUS.pending;
      }
    })
    .addCase(workingShift.fulfilled, (state, action) => {
      if (isEqual(state.loading, REQUEST_STATUS.pending)) {
        state.loading = REQUEST_STATUS.idle;
        state.workingShiftData = action.payload;
      }
    })
    .addCase(workingShift.rejected, (state, action) => {
      if (isEqual(state.loading, REQUEST_STATUS.pending)) {
        state.loading = REQUEST_STATUS.idle;
        state.error = action.error;
      }
    });
  }
});

const { actions, reducer } = workingShiftSlice;
export { actions, workingShift };

export default reducer;
