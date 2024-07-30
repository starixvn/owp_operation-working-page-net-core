import { configureStore } from '@reduxjs/toolkit';
import eventCheckingReducer from '../features/Table/eventSlice/eventCheckingSlice';
import chatBoardReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import signalRReducer from '../features/signalR/signalRSlice';

const store = configureStore({
  reducer: {
    eventChecking: eventCheckingReducer,
    chatBoard: chatBoardReducer,
    createTicketBoard: createTicketBoardReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    deletePolicy: createMultiplePolicySlice('deletePolicy'),
    signalR: signalRReducer
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
