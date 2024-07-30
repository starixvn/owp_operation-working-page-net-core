import { useEffect } from 'react';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { updateFilterDate } from '../filter/filterSlice';
import { buildFilterDate } from '../../ImportPrematchHelperNative';

const useBuildFilterDate = () => {
  const stableDispatch = useStableDispatch();
  return useEffect(() => {
    const filterDate = buildFilterDate();
    stableDispatch(updateFilterDate(filterDate));
  }, [stableDispatch]);
};

export default useBuildFilterDate;
