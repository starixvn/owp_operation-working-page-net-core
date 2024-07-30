import { configureStore } from '@reduxjs/toolkit';
import fileLoaderSlice from '../features/fileLoaderSlice';
import validationSlice from '../features/validationSlice';
import submitSlice from '../features/submitSlice';
import osAssignmentSlice from '../features/OSTable/OSTableSlice/reducer';
import filterHeaderSlice from '../features/FilterHeader/FilterHeaderSlice';
import lastUpdateSlice from '../features/LastUpdate/lastUpdateSlice';
import { isEqual } from 'lodash';

const store = configureStore({
  reducer: {
    fileLoader: fileLoaderSlice,
    validation: validationSlice,
    submit: submitSlice,
    osAssignmentTable: osAssignmentSlice,
    filterHeader: filterHeaderSlice,
    lastUpdate: lastUpdateSlice,
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
