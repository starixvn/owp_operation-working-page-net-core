import { createSelectorHook, useDispatch } from 'react-redux';
import type { RootState, AppDispatch } from './storeNative';

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector = createSelectorHook<RootState>();
