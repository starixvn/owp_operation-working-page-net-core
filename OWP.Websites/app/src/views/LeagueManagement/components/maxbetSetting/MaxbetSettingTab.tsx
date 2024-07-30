import { useEffect } from 'react';
import MaxbetSettingTable from './MaxbetSettingTable';
import EditPopup from './popup/EditPopup';
import LogSettingPopup from './popup/LogSettingPopup';
import useStore, { RootState } from './store/storeNative';

const MaxbetSettingTab = () => {
  const { queryPolicy } = useStore((state: RootState) => state);

  useEffect(() => {
    queryPolicy();
  }, []);

  return (
    <div>
      <MaxbetSettingTable />
      <EditPopup />
      <LogSettingPopup />
    </div>
  );
};

export default MaxbetSettingTab;
