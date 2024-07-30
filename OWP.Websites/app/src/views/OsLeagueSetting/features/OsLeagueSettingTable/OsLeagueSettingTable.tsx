import { useState, useEffect } from 'react';
import CustomTable from '../../../Shared/common/CustomTable/CustomTable';
import { DefaultTableStyle } from './TableSchema.json';
import useStore from '../../app/store';
import { calculateRowHeight, selectTable } from './OsLeagueSettingTableHelper';

const getTableSchema = () => {
  const { innerWidth } = window;
  return selectTable(innerWidth - DefaultTableStyle.widthResize);
};

const OsLeagueSettingTable = () => {
  const entities = useStore(state => state.entities);
  const loading = useStore(state => state.loading);
  const model = {
    loading,
    entities
  };
  const [tableSchema, setTableSchema] = useState(getTableSchema());
  const config = {
    ...tableSchema,
    onCalculateRowHeight: (index: number) => calculateRowHeight(entities[index])
  };

  const handleResizeBrowser = () => {
    const { innerWidth } = window;
    setTableSchema(selectTable(innerWidth - DefaultTableStyle.widthResize));
  };

  useEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  return <CustomTable config={config} model={model} />;
};

export default OsLeagueSettingTable;
