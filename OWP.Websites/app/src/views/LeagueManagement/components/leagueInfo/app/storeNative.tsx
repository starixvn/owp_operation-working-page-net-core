import { configureStore } from '@reduxjs/toolkit';
import LeagueInfoTableSlice from '../table/leagueInfoTableSlice/reducer';
import EditSettingSlice from '../editPopup/slice/editSettingSlice';
import AddSettingSlice from '../addPopup/slice/addSettingSlice';
const store = configureStore({
  reducer: {
    leagueInfoTableSlice: LeagueInfoTableSlice,
    editSettingSlice: EditSettingSlice,
    addSettingSlice: AddSettingSlice
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    }),

  devTools: true
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
