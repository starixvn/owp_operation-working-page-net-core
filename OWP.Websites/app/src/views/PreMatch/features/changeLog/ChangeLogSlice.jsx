/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { changeLog } from './ChangeLogThunkNative';

const ChangeLogSlice = createSlice({
  name: 'changeLog',
  initialState: {
    dataChangeLog: {
      Logs: null,
      IsLastPage: undefined // NOSONAR
    },
    loading: REQUEST_STATUS.idle,
    error: null
  },
  extraReducers: builder => {
    builder
      .addCase(changeLog.pending, state => {
        const { loading } = state;
        if (isEqual(loading, REQUEST_STATUS.idle)) {
          state.loading = REQUEST_STATUS.pending;
        }
      })
      .addCase(changeLog.fulfilled, (state, action) => {
        if (isEqual(state.loading, REQUEST_STATUS.pending)) {
          state.loading = REQUEST_STATUS.idle;
          state.dataChangeLog = action.payload;
        }
      })
      .addCase(changeLog.rejected, (state, action) => {
        if (isEqual(state.loading, REQUEST_STATUS.pending)) {
          state.loading = REQUEST_STATUS.idle;
          state.error = action.error;
        }
      });
  }
});

const { actions, reducer } = ChangeLogSlice;
export { actions, changeLog };

export default reducer;
