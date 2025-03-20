import { useState, useEffect } from 'react';
import CustomTable from '../../Shared/common/CustomTable/CustomTable';
import { DefaultTableStyle } from './TableSchema.json';
import { shallow } from 'zustand/shallow';

import { selectTable } from './UserManagementTableHelper';
import useStore from '../app/useStoreNative';
import { REQUEST_STATUS } from '../../Shared/Constants';
import Loading from '../../components/Loading';
const getTableSchema = () => {
  const { innerWidth } = window;
  return selectTable(innerWidth - DefaultTableStyle.widthResize);
};

const UserManagementTable = () => {
  const { queryDataTableStatus, dataSource } = useStore(
    state => ({
      queryDataTableStatus: state.queryDataTableStatus,
      dataSource: state.dataSource
    }),
    shallow
  );

  const [tableSchema, setTableSchema] = useState(getTableSchema());
  const model = {
    loading: queryDataTableStatus,
    entities: dataSource
  };

  const handleResizeBrowser = () => {
    const { innerWidth } = window;
    setTableSchema(selectTable(innerWidth - DefaultTableStyle.widthResize));
  };

  useEffect(() => {
    const abort = new AbortController();
    window.addEventListener('resize', handleResizeBrowser, abort);
    return () => abort.abort();
  }, []);

  if (queryDataTableStatus === REQUEST_STATUS.pending) {
    return <Loading />;
  }

  return <CustomTable config={tableSchema} model={model} />;
};

export default UserManagementTable;
