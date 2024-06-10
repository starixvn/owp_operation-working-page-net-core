import { createSlice } from '@reduxjs/toolkit';
import { buildIsReadValue } from '../../Shared/ticketEvent/helper/ticketHelper';

const tableSlice = createSlice({
  name: 'EventInformation',
  initialState: {
    filterDate: '',
    dataEvent: [],
    userId: 0
  },
  reducers: {
    setUserId: (state, action) => {
      state.userId = action.payload;
    },
    setDataEvent: (state, action) => {
      state.dataEvent = action.payload;
    },
    setDataSignalR: (state, action) => {
      const { actorId, parentMatchId } = action.payload;
      state.dataEvent.forEach(item => {
        if (item.ParentMatchId === parentMatchId) {
          item.IsRead = buildIsReadValue(actorId, state.userId, item.IsRead);
          item.HasTicket = true;
        }
      });
    },
    setFilterDate: (state, action) => {
      state.filterDate = action.payload;
    }
  }
});

const { actions, reducer } = tableSlice;
const { setDataEvent, setDataSignalR, setUserId, setFilterDate } = actions;
export { setDataEvent, setDataSignalR, setUserId, setFilterDate };
export default reducer;
