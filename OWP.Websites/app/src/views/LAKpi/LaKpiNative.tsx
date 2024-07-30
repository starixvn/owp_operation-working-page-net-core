import { useEffect } from 'react';
import LAKpiTable from './feature/table/LAKpiTable';
import Filter from './feature/filter/Filter';
import useStore from './app/store';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS } from '../Shared/Constants';
import Loading from '../components/Loading';

const LAKpiNative = () => {
  const queryPermissionsAndSections = useStore(
    state => state.queryPermissionsAndSections
  );
  const loading = useStore(state => state.loading);

  useEffect(() => {
    queryPermissionsAndSections();
  }, []);

  if (isEquals(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <Filter />
      <LAKpiTable />
    </>
  );
};

export default LAKpiNative;
