import { StateCreator } from 'zustand';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { TPolicy, TPermission } from '../type';
import axios from 'axios';
import update from 'immutability-helper';

const defaultPolicy: TPolicy = {
  isRead: false,
  isHost: false
};

const createPermissionSliceNative: StateCreator<
  TPermission,
  [],
  [],
  TPermission
> = (set, get) => ({
  queryPermissionStatus: REQUEST_STATUS.pending,
  permissions: {
    userManagement: defaultPolicy,
    userManagementOwnSection: defaultPolicy,
    userManagementAllSections: defaultPolicy
  },
  async queryPermissions() {
    const url =
      process.env.NODE_ENV === 'development'
        ? `api-netcore/UserManagement/query-permission`
        : `${window.netCoreRootUrl}/api/UserManagement/query-permission`;
    const response = await axios.get(url);
    set(state =>
      update(state, {
        queryPermissionStatus: { $set: REQUEST_STATUS.idle },
        permissions: { $set: response.data }
      }));

  }
});

export default createPermissionSliceNative;