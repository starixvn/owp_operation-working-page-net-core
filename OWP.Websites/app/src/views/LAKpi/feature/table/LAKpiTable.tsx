import { useEffect, useLayoutEffect, useRef, useState } from 'react';
import TableHeader from './TableHeader';
import TableBody from './TableBody';
import useStore from '../../app/store';
import Loading from '../../../components/Loading';
import { isEquals } from 'immutability-helper';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { headerHeight } from '../../TableConst.json';
import { CloseIcon } from 'mdi-react';
import { Button } from 'reactstrap';
import { filterData } from '../../helper/LaKpiHelper';

const LAKpiTable = () => {
  const [maxHeight, setMaxHeight] = useState(window.innerHeight - headerHeight);
  const [isOpen, setIsOpen] = useState(false);
  const [isActiveIconFilter, setIsActiveIconFilter] = useState(false);
  const { loadingStatus, entities, isGap, dataSourcesFiltered } = useStore(
    state => state.dataTable
  );
  const { filterHeaderValue } = useStore(state => state);
  const setDataSourcesFiltered = useStore(
    state => state.setDataSourcesFiltered
  );
  const setFilterHeader = useStore(state => state.setFilterHeader);
  const tableRef = useRef<HTMLDivElement>(null);
  const filterRef = useRef<HTMLDivElement>(null);

  const handleResizeBrowser = () => {
    const { innerHeight } = window;
    setMaxHeight(innerHeight - headerHeight);
  };

  useLayoutEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  useEffect(() => {
    const handleClickOutSide = (event: MouseEvent) => {
      if (
        filterRef.current &&
        !filterRef.current.contains(event.target as Node)
      ) {
        closeFilterHeader();
      }
    };
    document.addEventListener('mousedown', handleClickOutSide);
    return () => {
      document.removeEventListener('mousedown', handleClickOutSide);
    };
  }, [filterRef]);

  if (isEquals(loadingStatus, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  const openFilterHeader = () => setIsOpen(true);
  const closeFilterHeader = () => setIsOpen(false);

  const handleOnSearchFilter = () => {
    setIsActiveIconFilter(true);
    setIsOpen(false);
    const dataFiltered = filterData(entities, filterHeaderValue);
    setDataSourcesFiltered(dataFiltered);
  };

  const handleClearAllFilter = () => {
    setFilterHeader('');
    setIsActiveIconFilter(false);
    setDataSourcesFiltered(entities);
  };

  return (
    <>
      {isOpen && (
        <div ref={filterRef} className='filter-popup filter-popup is-show'>
          <span className='close-popup' onClick={closeFilterHeader}>
            <CloseIcon size={14} />
          </span>
          <input
            className='input-field'
            placeholder='Search'
            value={filterHeaderValue}
            autoFocus
            onChange={e => setFilterHeader(e.target.value)}
          />
          <span className='clear-all' onClick={handleClearAllFilter}>
            Clear All
          </span>
          <Button
            className='btn-search'
            disabled={!filterHeaderValue}
            onClick={handleOnSearchFilter}>
            Search
          </Button>
        </div>
      )}
      <div ref={tableRef} style={{ maxHeight }} className='table-content-wrap'>
        <table className='table-content'>
          <thead>
            <TableHeader
              isGap={isGap}
              isActiveIconFilter={isActiveIconFilter}
              openFilterHeader={openFilterHeader}
            />
          </thead>
          <tbody>
            <TableBody dataSource={dataSourcesFiltered} isGap={isGap} />
          </tbody>
        </table>
      </div>
    </>
  );
};

export default LAKpiTable;
