import { configureStore } from '@reduxjs/toolkit';
import createTicketBoardReducer from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import filterReducer from '../features/header/filter/filterSlice';
import ticketSliceReducer from '../features/table/slice/ticketSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';
const store = configureStore({
  reducer: {
    createTicketBoard: createTicketBoardReducer,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    tickerSlice: ticketSliceReducer,
    filter: filterReducer
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
