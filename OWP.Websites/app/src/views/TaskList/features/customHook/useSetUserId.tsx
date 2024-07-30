import { useEffect } from 'react';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setUserId } from '../table/taskListSlice';

const useSetUserId = (userId: number) => {
  const dispatch = useStableDispatch();
  useEffect(() => {
    dispatch(setUserId(userId));
  }, [dispatch, userId]);
};

export default useSetUserId;
