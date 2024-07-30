import { createSlice } from '@reduxjs/toolkit';

const signalRSlice = createSlice({
  name: 'signalR',
  initialState: {
    issueModel: null
  },
  reducers: {
    onClickIssueModel: {
      reducer: (state, action) => {
        state.issueModel = action.payload;
      }
    }
  }
});

const { actions, reducer } = signalRSlice;
const { onClickIssueModel } = actions;
export { actions, onClickIssueModel };

export default reducer;
