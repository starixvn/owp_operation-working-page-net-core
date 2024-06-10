import { createSlice } from '@reduxjs/toolkit';

const signalRSlice = createSlice({
  name: 'signalR',
  initialState: {
    filterModel: null,
    issueModel: null
  },
  reducers: {
    onSubmitFilterModel: {
      reducer: (state, action) => {
        state.filterModel = action.payload;
      }
    },
    onClickIssueModel: {
      reducer: (state, action) => {
        state.issueModel = action.payload;
      }
    }
  }
});

const { actions, reducer } = signalRSlice;
const { onSubmitFilterModel, onClickIssueModel } = actions;
export { actions, onSubmitFilterModel, onClickIssueModel };

export default reducer;
