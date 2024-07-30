import { create } from 'zustand';
import createFilterSlice from './createFilterSliceNative';
import createPermissionSlice from './createPermissionSliceNative';
import createDataTableSliceNative from './createDataTableSliceNative';
import createLevelSliceNative from './createLevelSliceNative';
import createSectionNameSliceNative from './createCurrentSectionSliceNative';
import { mountStoreDevtool } from 'simple-zustand-devtools';
import { TDataTable, TFilter, TLevelOption, TPermission, TSectionName } from '../type';

const useStore = create<TFilter & TPermission & TDataTable & TLevelOption & TSectionName>()(
  (...a) => ({
    ...createPermissionSlice(...a),
    ...createFilterSlice(...a),
    ...createDataTableSliceNative(...a),
    ...createLevelSliceNative(...a),
    ...createSectionNameSliceNative(...a)
  })
);

if (process.env.NODE_ENV === 'development') {
  mountStoreDevtool('user-management', useStore);
}

export default useStore;
