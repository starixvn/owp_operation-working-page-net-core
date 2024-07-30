import { createSelectorHook, useDispatch } from 'react-redux';
import { RootState, AppDispatch } from './storeNative';
import { createAsyncThunk } from '@reduxjs/toolkit';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector = createSelectorHook<RootState>();
export const createAppAsyncThunk = createAsyncThunk.withTypes<{
  state: RootState
  dispatch: AppDispatch
  rejectValue: string
}>();
