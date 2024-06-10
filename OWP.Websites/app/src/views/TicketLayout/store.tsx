import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import chatBoardReducer from '../Shared/chatBoardNew/chatBoardSlice';
import createMultiplePolicySlice from '../Shared/permission/multiplePolicySlice';
import signalRReducer from './SignalR/signalRSlice';
const store = configureStore({
  reducer: {
    chatBoard: chatBoardReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    signalR: signalRReducer
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    }),

  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
