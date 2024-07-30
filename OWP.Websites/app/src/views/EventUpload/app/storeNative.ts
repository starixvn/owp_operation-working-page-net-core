import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import uploadReducer from '../features/slice/eventUploadSlice';

const store = configureStore({
  reducer: {
    uploadReducer: uploadReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production'),
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false,
    }),
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
