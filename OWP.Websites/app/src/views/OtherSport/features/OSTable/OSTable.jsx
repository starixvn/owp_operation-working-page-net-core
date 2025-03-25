import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import CustomTable from '../../../Shared/common/CustomTable/CustomTable';
import {
  buildClassNameHightLightMatch,
  calculateRowHight,
  selectTable
} from './osTableHelper';

const getTableSchema = () => {
  const { innerWidth } = window;
  return selectTable(innerWidth - 140);
};

const OSTable = () => {
  const [tableSchema, setTableSchema] = useState(getTableSchema());
  const model = useSelector(state => state.osAssignmentTable);
  const { entities } = model;

  const config = {
    ...tableSchema,
    onCalculateRowHeight: index => calculateRowHight(entities[index]),
    onRowClassNameGetter: index =>
      buildClassNameHightLightMatch(entities[index])
  };

  const handleResizeBrowser = () => {
    const { innerWidth } = window;
    setTableSchema(selectTable(innerWidth - 140));
  };

  useEffect(() => {
    const abort = new AbortController();
    window.addEventListener('resize', handleResizeBrowser, abort);
    return () => abort.abort();
  }, []);

  return <CustomTable model={model} config={config} />;
};

export default OSTable;
