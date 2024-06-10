import { useEffect, useState } from 'react';
import { shallow } from 'zustand/shallow';
import getPermission from '../Shared/permission/getPermission';
import Loading from '../components/Loading';
import useStore from './app/store';
import OsLeagueSettingTable from './features/OsLeagueSettingTable/OsLeagueSettingTable';
import FilterManagementNative from './features/filter/FilterManagementNative';
import PopupWrapper from './features/popup/PopupWrapper';
import { functionKey, localStoreKey } from './osLeague.json';

const OsLeagueSetting = () => {
  const [loading, setLoading] = useState(true);

  const { setPolicy } = useStore(state => (
    {
      setPolicy: state.setPolicy
    }), shallow);

  useEffect(() => {
    (async () => {
      const policy = await getPermission(functionKey);
      setPolicy(policy);
      setLoading(false);
    })();

    for (const key in localStoreKey) {
      localStorage.removeItem(localStoreKey[key as keyof typeof localStoreKey]);
    }
  }, []);

  if (loading) {
    return <Loading />;
  }

  return (
    <div className='os-league-setting'>
      <FilterManagementNative />
      <PopupWrapper />
      <OsLeagueSettingTable />
    </div>
  );
};

export default OsLeagueSetting;
