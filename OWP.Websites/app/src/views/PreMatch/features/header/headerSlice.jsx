/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getLastUpdate } from './headerThunkNative';

const headerSlice = createSlice({
  name: 'headerSlice',
  initialState: {
    info: {
      UpdatedTime: undefined, // NOSONAR
      Username: undefined // NOSONAR
    },
    loading: REQUEST_STATUS.idle,
    error: null,
    startTimeOriginal: undefined, // NOSONAR
    startTimeFilter: undefined // NOSONAR
  },
  reducers: {
    updateStartTimeFilter: {
      reducer: (state, action) => {
        state.startTimeFilter = action.payload;
      }
    },
    updateStartTimeOriginal: {
      reducer: (state, action) => {
        state.startTimeOriginal = action.payload;
        state.startTimeFilter = action.payload;
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getLastUpdate.pending, state => {
        const { loading } = state;
        if (isEqual(loading, REQUEST_STATUS.idle)) {
          state.loading = REQUEST_STATUS.pending;
        }
      })
      .addCase(getLastUpdate.fulfilled, (state, action) => {
        if (isEqual(state.loading, REQUEST_STATUS.pending)) {
          state.loading = REQUEST_STATUS.idle;
          const data = action.payload[0];
          if (data) {
            state.info = data;
          }
        }
      })
      .addCase(getLastUpdate.rejected, (state, action) => {
        if (isEqual(state.loading, REQUEST_STATUS.pending)) {
          state.loading = REQUEST_STATUS.idle;
          state.error = action.error;
        }
      });
  }
});

const { actions, reducer } = headerSlice;
const { updateStartTimeFilter, updateStartTimeOriginal } = actions;
export {
  actions,
  getLastUpdate,
  updateStartTimeFilter,
  updateStartTimeOriginal
};

export default reducer;
