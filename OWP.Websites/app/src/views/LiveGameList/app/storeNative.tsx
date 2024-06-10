import { configureStore } from '@reduxjs/toolkit';
import chatBoardReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import eventReducer from '../table/tableSlice/tableSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';

import { isEqual } from 'lodash';

const store = configureStore({
  reducer: {
    chatBoard: chatBoardReducer,
    createTicketBoard: createTicketBoardReducer,
    ticketList: ticketListSlice,
    signalREventTicket: signalREventTicketSlice,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    events: eventReducer
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
