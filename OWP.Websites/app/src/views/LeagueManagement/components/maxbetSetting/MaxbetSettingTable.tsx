import { isEqual } from 'lodash';
import { useEffect } from 'react';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import useStore, { RootState } from './store/storeNative';
import { renderColumn } from './Column';

const ReactTableFixedColumns = withFixedColumns(ReactTable);


const MaxbetSettingTable = () => {
  const {
    getMaxbetSettings,
    getSecondAcceptValues,
    maxbetSettings,
    loading
  } = useStore<RootState>(state => state);

  useEffect(() => {
    getMaxbetSettings();
    getSecondAcceptValues();
  }, []);

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      {maxbetSettings && (
        <div className='maxbet-setting-table'>
          <ReactTableFixedColumns
            data={maxbetSettings}
            noDataText='There is no data.'
            columns={renderColumn()}
            resizable={false}
            sortable={false}
            filterable={false}
            pageSize={maxbetSettings.length}
            minRows={3}
            className='-highlight -striped'
            showPagination={false}
          />
          <div className='total-records'>
            Total records: {maxbetSettings.length}
          </div>
        </div>
      )}
    </>
  );
};

export default MaxbetSettingTable;
