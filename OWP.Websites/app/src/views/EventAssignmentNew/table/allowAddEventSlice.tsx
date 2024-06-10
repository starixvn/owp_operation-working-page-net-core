import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import {
  TAllowAddEvent
} from '../type/type';
import Utils from '../../Shared/Utils';
import dayjs from 'dayjs';

const allowAddEventSlice = createSlice({
  name: 'allowAddEventSlice',
  initialState: {
    isYesterdayOrAfter: false,
    isAllowAddEvent: false
  } as TAllowAddEvent,
  reducers: {
    setAllowAddEvent: (state, action: PayloadAction<Date>) => {
      state.isYesterdayOrAfter =
        Utils.isYesterdayOrAfterForAssignment(action.payload);
      const today = new Date();
      const yesterDay = dayjs(today).subtract(1, 'day');
      state.isAllowAddEvent = Utils.isSameOrAfter(action.payload, yesterDay);
    }
  }
});
const { reducer, actions } = allowAddEventSlice;
const {
  setAllowAddEvent
} = actions;

export {
  setAllowAddEvent
};
export default reducer;
