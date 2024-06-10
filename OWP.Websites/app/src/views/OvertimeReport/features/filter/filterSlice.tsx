import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getLocationOptions } from '../overTimeReportThunkNative';
import { SelectOption } from '../typeNative';

const filterSlice = createSlice({
  name: 'filter',
  initialState: {
    status: REQUEST_STATUS.pending,
    locations: [] as SelectOption[]
  },
  reducers: {},
  extraReducers: builder => {
    builder.addCase(getLocationOptions.pending, state => {
      state.status = REQUEST_STATUS.pending;
    })
      .addCase(getLocationOptions.fulfilled, (state, { payload }) => {
        state.status = REQUEST_STATUS.idle;
        state.locations = payload;
      })
      .addCase(getLocationOptions.rejected, state => {
        state.status = REQUEST_STATUS.idle;
      });
  }
});

const { reducer } = filterSlice;

export default reducer;
