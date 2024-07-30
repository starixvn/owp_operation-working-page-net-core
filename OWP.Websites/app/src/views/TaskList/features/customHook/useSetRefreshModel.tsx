import { useEffect } from 'react';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { RefreshModel } from '../../type/Header';
import { setRefreshModel } from '../table/taskListSlice';

const useSetRefreshModel = ({ filterDate, isToday }: RefreshModel) => {
  const dispatch = useStableDispatch();
  useEffect(() => {
    dispatch(setRefreshModel({ filterDate, isToday }));
  }, []);
};

export default useSetRefreshModel;
