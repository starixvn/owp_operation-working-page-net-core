import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import filterReducer from '../features/filter/filterSlice';
import monthOTReducer from '../features/ByMonthTab/Helper/byMonthSlice';
import yearTabReducer from '../features/YearTabContent/yearTabSlice';

const store = configureStore({
  reducer: {
    filter: filterReducer,
    monthOTReducer,
    yearTab: yearTabReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
