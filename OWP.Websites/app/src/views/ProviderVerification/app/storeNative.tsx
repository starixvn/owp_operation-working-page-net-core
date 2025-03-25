import { configureStore } from '@reduxjs/toolkit';
import eventReducer from '../feature/table/tableSlice';
import filterReducer from '../feature/filter/FilterSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import providerSumReducer from '../feature/summarizeProvider/summarizeProviderSlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import chatBoardNew from '../../Shared/chatBoardNew/chatBoardSlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';
import commonSlice from '../feature/common/commonSlice';

const store = configureStore({
  reducer: {
    events: eventReducer,
    filterState: filterReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    providerPolicy: createMultiplePolicySlice('providerPolicy'),
    providerSum: providerSumReducer,
    ticketList: ticketListSlice,
    createTicketBoard: createTicketBoardReducer,
    chatBoard: chatBoardNew,
    signalREventTicket: signalREventTicketSlice,
    common: commonSlice
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    })
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
