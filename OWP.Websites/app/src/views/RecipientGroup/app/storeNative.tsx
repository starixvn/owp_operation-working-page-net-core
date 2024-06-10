import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import policyReducer from '../../Shared/permission/policySlice';
import groupReducer from '../features/groupSlice';

const store = configureStore({
  reducer: {
    policy: policyReducer,
    recipientGroup: groupReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
