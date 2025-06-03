import { useState } from 'react';
import { post } from '../../Shared/Service/Service';
import { buildQuerySimpleApi } from '../../Shared/Constants';
import UserDetail from './UserDetail';
import { confirmSyncUserEnum } from '../type';
import ToastrWrapper from '../../components/ToastrWrapper';
import store from '../app/useStoreNative';
import axios from 'axios';

const SyncUser = () => {
  const [displayUserDetail, setDisplayUserDetail] = useState(false);
  const [userDetail, setUserDetail] = useState([]);
  const { permissions } = store.getState();
  const { userManagementAllSections } = permissions;

  const handleQueryUserFromAdminSite = async () => {
    const response = await post({
      url: buildQuerySimpleApi(),
      apiKey: 'get-user-change-from-admin-site'
    });

    if (!response.data.length) {
      ToastrWrapper.showInfo("Not found any user to sync");
      return;
    }
    setUserDetail(response.data);
    setDisplayUserDetail(true);
  };

  const handleSyncUser = async (value: confirmSyncUserEnum) => {
    if (value === confirmSyncUserEnum.save) {
      await post({
        url: buildQuerySimpleApi(),
        apiKey: 'sync-user-change-from-admin-site'
      });
      await axios.post('/api/owp/chat/update-user-name', userDetail);
      ToastrWrapper.showInfo("Sync users successful");
    }
    setDisplayUserDetail(false);
  };

  return (
    <>
      {userManagementAllSections.isHost && <div className='sync__user'>
        <div className='sync__user__block'>
          <button
            className='button--sync__user'
            onClick={handleQueryUserFromAdminSite}>
            Sync User
          </button>
        </div>
      </div>}
      {displayUserDetail &&
        <UserDetail
          onToggle={handleSyncUser}
          dataSource={userDetail}
          hasWrite
        />
      }
    </>
  );
};


export default SyncUser;