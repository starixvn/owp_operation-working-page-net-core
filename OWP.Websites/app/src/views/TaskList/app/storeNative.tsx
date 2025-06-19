import { configureStore } from '@reduxjs/toolkit';
import taskListReducer from '../features/table/taskListSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import chatBoardReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';
import tradingPageInfoSlice from '../../Shared/tradingPage/tradingPageInfoSlice';

const store = configureStore({
  reducer: {
    taskList: taskListReducer,
    chatBoard: chatBoardReducer,
    createTicketBoard: createTicketBoardReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    ticketList: ticketListSlice,
    signalREventTicket: signalREventTicketSlice,
    tradingPageInfo: tradingPageInfoSlice
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
