import { create } from 'zustand';
import {
  TQueryWorkScheduleInfo,
  TQueryColorExplainInfo
} from '../features/TypeNative';
import createTableSlice from './createTableSliceNative';

import createColorExplainSlice from './createColorExplainSliceNative';

export default create<TQueryWorkScheduleInfo & TQueryColorExplainInfo>()(
  (...a) => ({
    ...createTableSlice(...a),
    ...createColorExplainSlice(...a)
  })
);
