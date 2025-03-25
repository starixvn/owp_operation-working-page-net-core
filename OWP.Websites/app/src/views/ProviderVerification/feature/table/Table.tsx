import { useAppSelector } from '../../app/hookNative';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Loading from '../../../components/Loading';
import TableHeader from './TableHeader';
import TableBody from './TableBody';
import useLayoutTable from '../../../Shared/customHook/useLayoutTable';

const Table = () => {
  const { dataSource, loadingDataTable } = useAppSelector(
    state => state.events
  );
  const { maxHeight, maxWidth } = useLayoutTable();

  if (isEquals(loadingDataTable, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <div
        className='table-wrap'
        style={{ maxHeight, maxWidth, minHeight: '650px' }}>
        <table className='table__provider__verification'>
          <TableHeader />
          <TableBody dataSource={dataSource} />
        </table>
      </div>
      <div className='total-record'>Total record: {dataSource.length}</div>
    </>
  );
};

export default Table;
