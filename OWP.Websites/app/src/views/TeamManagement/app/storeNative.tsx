import { configureStore } from '@reduxjs/toolkit';
import filterReducer from '../features/filter/FilterSlice';
import teamsReducer from '../features/table/tableSlice';


const store = configureStore({
  reducer: {
    teams: teamsReducer,
    filterState: filterReducer
  }
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
