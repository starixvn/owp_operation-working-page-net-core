import { useDispatch } from 'react-redux';
import { useCallback } from 'react';

const useStableDispatch = () => {
  const dispatch = useDispatch();
  return useCallback(dispatch, [dispatch]);
};

export default useStableDispatch;
