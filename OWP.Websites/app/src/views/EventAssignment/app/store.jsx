import { configureStore } from '@reduxjs/toolkit';
import chatBoardNew from '../../Shared/chatBoardNew/chatBoardSlice';
import createTicketBoardReducerNew from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import policySlice from '../../Shared/permission/policySlice';
import ticketListSlice from '../../Shared/ticketEvent/component/ticketListSlice';
import signalREventTicketSlice from '../../Shared/ticketEvent/signalR/signalREventTicketSlice';
import tableReducer from '../reducer/tableReducer';
import { isEqual } from 'lodash';

const store = configureStore({
  reducer: {
    chatBoard: chatBoardNew,
    createTicketBoard: createTicketBoardReducerNew,
    pagePolicy: policySlice,
    ticketList: ticketListSlice,
    signalREventTicket: signalREventTicketSlice,
    tableReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export default store;
