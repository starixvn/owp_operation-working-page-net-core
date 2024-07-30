import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import reportReducer from '../features/table/reportSlice';

const store = configureStore({
  reducer: {
    report: reportReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
