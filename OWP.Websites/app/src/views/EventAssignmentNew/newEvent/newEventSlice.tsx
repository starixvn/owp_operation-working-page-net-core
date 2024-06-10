import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { TDataSource } from '../type/type';

const newEventSlice = createSlice({
  name: 'newEventSlice',
  initialState: {
    event: {} as TDataSource,
  },
  reducers: {
    setNewEventInfo: (state, action: PayloadAction<TDataSource>) => {
      state.event = action.payload;
    }
  }
});
const { reducer, actions } = newEventSlice;
const { setNewEventInfo } = actions;
export { setNewEventInfo };
export default reducer;
