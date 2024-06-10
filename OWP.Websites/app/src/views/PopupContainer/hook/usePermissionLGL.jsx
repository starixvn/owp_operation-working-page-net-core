import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { queryPolicy } from '../permission/multiplePolicySlice';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';

const usePermissionLGL = (functionKey, url, policy = 'policy') => {
  const stableDispatch = useStableDispatch();
  const permission = useSelector(state => state[policy]);

  useEffect(() => {
    stableDispatch(queryPolicy(policy, url)({ functionKey }));
  }, [stableDispatch, functionKey, policy, url]);

  return permission;
};

export default usePermissionLGL;
