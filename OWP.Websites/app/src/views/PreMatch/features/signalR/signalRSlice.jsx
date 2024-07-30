/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const signalRSlice = createSlice({
  name: 'signalR',
  initialState: {
    backupFilterModel: null,
    backupGetIssueModel: null,
    loading: REQUEST_STATUS.idle,
    currentRequestId: undefined, // NOSONAR
    error: null
  },
  reducers: {
    backupFilterModel: {
      reducer: (state, action) => {
        state.backupFilterModel = action.payload;
      }
    },
    backupGetIssueModel: {
      reducer: (state, action) => {
        state.backupGetIssueModel = action.payload;
      }
    }
  }
});

const { actions, reducer } = signalRSlice;
const { backupFilterModel, backupGetIssueModel } = actions;
export { actions, backupFilterModel, backupGetIssueModel };

export default reducer;
