import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import policyReducer from '../../Shared/permission/policySlice';
import ticketTypeReducer from '../features/ticketTypeSlice';

const store = configureStore({
  reducer: {
    policy: policyReducer,
    ticketTypeRecipient: ticketTypeReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
