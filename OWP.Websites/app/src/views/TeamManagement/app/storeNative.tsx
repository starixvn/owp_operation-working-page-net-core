import { configureStore } from '@reduxjs/toolkit';
import leaguesReducer from '../features/table/tableSlice';

const store = configureStore({
  reducer: {
    leagues: leaguesReducer,
  }
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
