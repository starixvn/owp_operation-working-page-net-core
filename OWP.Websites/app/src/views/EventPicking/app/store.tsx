import { configureStore } from '@reduxjs/toolkit';
import EventPickingTableSlice from '../table/eventPickingTableSlice/reducer';
import filterSlice from '../filterArea/slice/filterSlice';
import modeSlice from '../mode/modeSlice';
import sumProviderSlice from '../sumProvider/slice/sumProviderSlice';
import createMultiplePolicySlice from '../../Shared/permission/multiplePolicySlice';

const store = configureStore({
  reducer: {
    eventPickingTable: EventPickingTableSlice,
    pagePolicy: createMultiplePolicySlice('pagePolicy'),
    turnOverMarginPolicy: createMultiplePolicySlice('turnOverMarginPolicy'),
    filter: filterSlice,
    sumProvider: sumProviderSlice,
    mode: modeSlice
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
