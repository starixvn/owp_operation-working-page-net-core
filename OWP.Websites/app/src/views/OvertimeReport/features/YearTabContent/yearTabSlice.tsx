import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getReportByYear } from '../overTimeReportThunkNative';
import { OverTimeDataByYear } from '../typeNative';

const yearTabSlice = createSlice({
  name: 'yearTab',
  initialState: {
    status: REQUEST_STATUS.pending,
    overtimeData: [] as OverTimeDataByYear[]
  },
  reducers: {},
  extraReducers: builder => {
    builder.addCase(getReportByYear.pending, state => {
      state.status = REQUEST_STATUS.pending;
    })
      .addCase(getReportByYear.fulfilled, (state, { payload }) => {
        state.status = REQUEST_STATUS.idle;
        state.overtimeData = payload;
      })
      .addCase(getReportByYear.rejected, state => {
        state.status = REQUEST_STATUS.idle;
      });
  }
});

const { reducer } = yearTabSlice;

export default reducer;
