import { configureStore } from '@reduxjs/toolkit';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import chatBoardReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import createPolicySlice from '../permission/multiplePolicySlice';
import signalRReducer from '../signalR/signalRSlice';
import { isEqual } from 'lodash';

const store = configureStore({
  reducer: {
    createTicketBoard: createTicketBoardReducer,
    chatBoard: chatBoardReducer,
    signalR: signalRReducer,
    policy: createPolicySlice('policy')
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
