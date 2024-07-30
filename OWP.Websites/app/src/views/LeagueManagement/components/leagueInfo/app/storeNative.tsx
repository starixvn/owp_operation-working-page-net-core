import { configureStore } from '@reduxjs/toolkit';
import LeagueInfoTableSlice from '../table/leagueInfoTableSlice/reducer';
import EditSettingSlice from '../editPopup/slice/editSettingSlice';

const store = configureStore({
  reducer: {
    leagueInfoTableSlice: LeagueInfoTableSlice,
    editSettingSlice: EditSettingSlice
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
