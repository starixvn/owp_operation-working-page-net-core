import { createSlice } from '@reduxjs/toolkit';
import { getReportByMonth } from './monthTabThunkNative';
import { MonthOvertimeData } from './TypeNative';

const monthReportSlice = createSlice({
  name: 'monthReportSlice',
  initialState: {
    isLoading: true,
    data: {} as MonthOvertimeData,
    isFulfill: false
  },
  reducers: {},
  extraReducers: builder => {
    builder.addCase(getReportByMonth.pending, state => {
      state.isLoading = true;
    })
      .addCase(getReportByMonth.fulfilled, (state, { payload }) => {
        state.isLoading = false;
        state.isFulfill = true;
        state.data = payload;
      })
      .addCase(getReportByMonth.rejected, state => {
        state.isLoading = false;
      });
  }
});

const { reducer } = monthReportSlice;

export {
  getReportByMonth
};

export default reducer;
