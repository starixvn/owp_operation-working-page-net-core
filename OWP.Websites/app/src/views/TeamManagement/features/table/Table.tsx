import { useAppSelector } from '../../app/hookNative';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Loading from '../../../components/Loading';
import TableHeader from './TableHeader';
import useLayoutTable from '../../../Shared/customHook/useLayoutTable';
import { useRef } from 'react';
import TableBody from './TableBody';
import { useVirtualizer } from '@tanstack/react-virtual';

const ROW_HEIGHT = 40;

const Table = () => {
  const { leagues, loadingDataTable } = useAppSelector(
    state => state.leagues
  );
  const { maxHeight, maxWidth } = useLayoutTable();
  const parentRef = useRef<HTMLDivElement>(null);

  const rowVirtualizer = useVirtualizer({
    count: leagues.length,
    getScrollElement: () => parentRef.current,
    estimateSize: () => ROW_HEIGHT,
    overscan: 5
  });

  if (isEquals(loadingDataTable, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <div
        ref={parentRef}
        className='table-wrap'
        style={{
          maxHeight,
          maxWidth,
          overflow: 'auto'
        }}>
        {!isEquals(loadingDataTable, 'first-time') &&
          <table className='table-team-management'>
            <TableHeader />
            <TableBody dataSource={leagues} virtualizer={rowVirtualizer} />
          </table>}
      </div>
      <div className='total-record'>Total record: {leagues.length}</div>
    </>
  );
};

export default Table;
