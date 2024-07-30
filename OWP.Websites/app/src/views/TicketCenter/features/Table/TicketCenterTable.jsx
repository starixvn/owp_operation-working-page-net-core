import { useEffect, useState } from 'react';
import { useAppSelector } from '../../app/hooksNative';
import CustomTable from '../../../Shared/common/CustomTable/CustomTable';
import { getConfigTicketsTab } from '../Table/ConfigNative';

const TicketCenterTable = () => {
  const model = useAppSelector(state => state.eventChecking);
  const [tableSchema, setTableSchema] = useState(
    getConfigTicketsTab(window.innerWidth)
  );
  const handleResizeBrowser = () => {
    const newConfig = getConfigTicketsTab(window.innerWidth);
    setTableSchema(newConfig);
  };

  useEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  return <CustomTable model={model} config={tableSchema} />;
};

export default TicketCenterTable;
