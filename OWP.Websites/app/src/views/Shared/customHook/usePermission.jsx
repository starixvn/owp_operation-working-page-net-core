import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { queryPolicy } from '../permission/policySlice';
import useStableDispatch from '../customHook/useStableDispatch';

const usePermission = (functionKey, policy = 'policy') => {
  const stableDispatch = useStableDispatch();
  const permission = useSelector(state => state[policy]);

  useEffect(() => {
    stableDispatch(queryPolicy(policy)({ functionKey }));
  }, [stableDispatch, functionKey, policy]);

  return permission;
};

export default usePermission;
