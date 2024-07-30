import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import NetCoreSignalR from '../../../Shared/NetCoreSignalR/NetCoreSignalR';
import getOSAssignmentThunkNative from '../OSTable/OSTableSlice/getOSAssignmentThunkNative';
import { useStore } from 'react-redux';
import {useAppDispatch} from '../../app/hooks';
import getLastUpdateThunkNative from '../../features/LastUpdate/getLastUpdateThunkNative';

const AutoRefresh = () => {
  const store = useStore();
  const stableDispatch = useStableDispatch();
  const dispatch = useAppDispatch();

  const handleGetOSAssignment = () => {
    const state = store.getState();
    const { model } = state.filterHeader;
    dispatch(getLastUpdateThunkNative());
    stableDispatch(getOSAssignmentThunkNative(model));
  };
  const handlers = [
    {
      method: 'NotifyUploadedNewData',
      handler: handleGetOSAssignment
    }
  ];

  return <NetCoreSignalR
    handlers={handlers}
    hubName='otherSportNewDataUploadNotifyHub'
  />;
};

export default AutoRefresh;
