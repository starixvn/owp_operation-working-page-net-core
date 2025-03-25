import { configureStore } from '@reduxjs/toolkit';
import uploadReducer from '../feature/uploadProvider/providerUploadSlice';
import eventReducer from '../feature/table/tableSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import chatBoardNew from '../../Shared/chatBoardNew/chatBoardSlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';
import commonSlice from '../feature/common/commonSlice';

const store = configureStore({
  reducer: {
    upload: uploadReducer,
    events: eventReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    providerPolicy: createMultiplePolicySlice('providerPolicy'),
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
