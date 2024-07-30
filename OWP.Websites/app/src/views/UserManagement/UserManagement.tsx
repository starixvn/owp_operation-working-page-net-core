import { useEffect } from 'react';
import Filter from './filterArea/Filter';
import SyncUser from './syncUser/SyncUser';
import useStore from './app/useStoreNative';
import { REQUEST_STATUS } from '../Shared/Constants';
import Loading from '../components/Loading';
import { shallow } from 'zustand/shallow';
import UserManagementTable from './table/UserManagementTable';

const UserManagement = () => {
  const {
    queryFilterOptionsStatus,
    queryPermissionStatus,
    queryLevelOptionStatus,
    queryFilterOptions,
    queryLevelOption,
    queryPermissions,
    querySectionName
  } = useStore(
    state => ({
      queryFilterOptionsStatus: state.queryFilterOptionsStatus,
      queryPermissionStatus: state.queryPermissionStatus,
      queryLevelOptionStatus: state.queryLevelOptionStatus,
      queryFilterOptions: state.queryFilterOptions,
      queryLevelOption: state.queryLevelOption,
      queryPermissions: state.queryPermissions,
      querySectionName: state.querySectionName
    }),
    shallow
  );

  useEffect(() => {
    querySectionName();
    queryFilterOptions();
    queryPermissions();
    queryLevelOption();
  }, [queryFilterOptions, queryLevelOption, queryPermissions, querySectionName]);

  if (
    [queryFilterOptionsStatus, queryPermissionStatus, queryLevelOptionStatus].some(
      status => status === REQUEST_STATUS.pending
    )
  ) {
    return <Loading />;
  }

  return (
    <div className='user__management'>
      <div className='user__management--header'>
        <Filter />
        <SyncUser />
      </div>
      <div>
        <UserManagementTable />
      </div>
    </div>
  );
};

export default UserManagement;
