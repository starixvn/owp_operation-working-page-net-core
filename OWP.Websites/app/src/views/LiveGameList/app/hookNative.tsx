import { createSelectorHook, useDispatch, useSelector } from 'react-redux';
import type { RootState, AppDispatch } from './storeNative';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector = createSelectorHook<RootState>();
