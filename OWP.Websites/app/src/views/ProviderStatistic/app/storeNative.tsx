import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';

const store = configureStore({
  reducer: {
    // some: someReducer,
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
