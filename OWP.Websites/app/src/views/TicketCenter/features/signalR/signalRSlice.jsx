import { createSlice } from '@reduxjs/toolkit';

const signalRSlice = createSlice({
  name: 'signalR',
  initialState: {
    filterModel: null,
    issueModel: null,
    activeTab: 'TAB_MY_TICKET'
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
    },
    setActiveTabSignalR: {
      reducer: (state, action) => {
        state.activeTab = action.payload;
      }
    }
  }
});

const { actions, reducer } = signalRSlice;
const { onSubmitFilterModel, onClickIssueModel, setActiveTabSignalR } = actions;
export { actions, onSubmitFilterModel, onClickIssueModel, setActiveTabSignalR };

export default reducer;
