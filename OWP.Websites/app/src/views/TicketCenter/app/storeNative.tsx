import { configureStore } from '@reduxjs/toolkit';
import eventCheckingReducer from '../features/Table/eventSlice/eventCheckingSlice';
import chatBoardReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import signalRReducer from '../features/signalR/signalRSlice';
import subtypeReducer from '../features/consolidation/subtype/subtypeSlice';
import consolidationSlice from '../features/consolidation/consolidationView/consolidationSlice';
const store = configureStore({
  reducer: {
    eventChecking: eventCheckingReducer,
    chatBoard: chatBoardReducer,
    createTicketBoard: createTicketBoardReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    consolidationPolicy: createMultiplePolicySlice('consolidationPolicy'),
    deletePolicy: createMultiplePolicySlice('deletePolicy'),
    signalR: signalRReducer,
    subtype: subtypeReducer,
    consolidation: consolidationSlice
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
