import { configureStore } from '@reduxjs/toolkit';
import uploadReducer from '../feature/uploadProvider/providerUploadSlice';

const store = configureStore({
  reducer: {
    upload: uploadReducer
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    })
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
