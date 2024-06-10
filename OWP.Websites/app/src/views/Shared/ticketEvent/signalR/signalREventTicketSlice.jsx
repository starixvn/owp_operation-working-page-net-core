import { createSlice } from '@reduxjs/toolkit';

const signalREventTicketSlice = createSlice({
  name: 'signalREventTicketSlice',
  initialState: {
    userId: null,
    issueModel: null,
    owpMatchId: null
  },
  reducers: {
    onClickIssueModel: {
      reducer: (state, action) => {
        state.issueModel = action.payload;
      }
    },
    setUserId: {
      reducer: (state, action) => {
        state.userId = action.payload;
      }
    },
    setOwpMatchId: {
      reducer: (state, action) => {
        state.owpMatchId = action.payload;
      }
    }
  }
});

const { actions, reducer } = signalREventTicketSlice;
const { setUserId, onClickIssueModel, setOwpMatchId } = actions;
export { actions, setUserId, onClickIssueModel, setOwpMatchId };

export default reducer;
