import { configureStore } from '@reduxjs/toolkit';
import reducer from '../features/quickMessage/slice/quickMessageSlice';
import adminServerSettingReducer from '../features/adminServerSetting/slice/adminServerSettingSlice';
const store = configureStore({
  reducer: {
    quickMessageSlice: reducer,
    adminServerSettingSlice: adminServerSettingReducer
  },
  middleware: getDefaultMiddleware => getDefaultMiddleware({
    serializableCheck: false
  }),
  devTools: true
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;