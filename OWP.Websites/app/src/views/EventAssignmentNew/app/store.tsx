import { configureStore } from '@reduxjs/toolkit';
import eventAssignmentTableSlice from '../table/eventAssignmentTableSlice';
import allowAddEventSlice from '../table/allowAddEventSlice';
import policySlice from '../policy/policySlice';
import filterSlice from '../filter/filterSlice';
import newEventSlice from '../newEvent/newEventSlice';
import eventDetailSlice from '../eventDetail/eventDetailSlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import chatBoardNew from '../../Shared/chatBoardNew/chatBoardSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';
import tradingPageInfoSlice from '../../Shared/tradingPage/tradingPageInfoSlice';
const store = configureStore({
  reducer: {
    eventAssignmentTable: eventAssignmentTableSlice,
    allowAddEvent: allowAddEventSlice,
    policy: policySlice,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    filter: filterSlice,
    ticketList: ticketListSlice,
    chatBoard: chatBoardNew,
    newEvent: newEventSlice,
    eventDetail: eventDetailSlice,
    createTicketBoard: createTicketBoardReducer,
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
