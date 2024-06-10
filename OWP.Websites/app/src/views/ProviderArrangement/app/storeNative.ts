import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import providerSlice from '../features/ProviderArrangementSlice';

const store = configureStore({
  reducer: {
    providerReducer: providerSlice
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
