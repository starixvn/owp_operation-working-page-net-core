import { isEqual } from 'lodash';
import { useEffect } from 'react';
import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { maxbetSettingTable } from './Const.json';
import useStore, { RootState } from './store/storeNative';
import { ColumnModel, SubColumnModel } from './typeNative';
import Cell from './Cell';

const ReactTableFixedColumns = withFixedColumns(ReactTable);
const {
  fullTimeMaxbetRefer,
  mainMatch,
  noOfCorners,
  nextCorners,
  totalBookings,
  nextBooking,
  secondAcceptValue,
  lastUpdate
} = maxbetSettingTable;
const groupColumns = [
  mainMatch,
  noOfCorners,
  nextCorners,
  totalBookings,
  nextBooking,
  secondAcceptValue
];

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

  const renderFixedColumns = (column: ColumnModel) => {
    const renderFixedColumn = (subCol: SubColumnModel) => {
      return {
        Header: subCol.name,
        headerClassName: `${subCol.id}-header`,
        id: `${subCol.id}`,
        accessor: `${subCol.id}`,
        className: `${subCol.id}-value`,
        Cell: (cellInfo: CellInfo) => <Cell
          value={cellInfo.original[subCol.id]}
          id={subCol.id}
          maxBetValue={cellInfo.row.fT_HDP}
        />
      };
    };

    return [
      {
        Header: column.name,
        accessor: '',
        headerClassName: `${column.id}-group-header`,
        fixed: column.fixed,
        id: column.id,
        columns: [renderFixedColumn(column.subColumn[0])]
      }
    ];
  };

  const renderGroupColumn = () => {
    const renderSubColumn = (
      subColumns: SubColumnModel[],
      width: number,
      headerClassName: string) => {
      return subColumns.map(subCol => {
        return {
          Header: subCol.name,
          headerClassName: `${headerClassName}-header`,
          accessor: `${subCol.id}`,
          width,
          id: `${subCol.id}`,
          Cell: (cellInfo: CellInfo) => <Cell
            value={cellInfo.original[`${subCol.id}`]}
            id={subCol.id}
            maxBetValue={cellInfo.row.fT_HDP} />
        };
      });
    };

    return groupColumns.map(column => {
      return {
        Header: column.name,
        headerClassName: `${column.id}-group-header`,
        accessor: column.name,
        id: column.id,
        columns: renderSubColumn(column.subColumn, column.subColWidth, column.headerClassName)
      };
    });
  };

  const renderColumn = () => {
    const fullTimeMaxbetReferCol = renderFixedColumns(fullTimeMaxbetRefer);
    const groupCols = renderGroupColumn();
    const lastUpdatedFixedCol = renderFixedColumns(lastUpdate);
    return [
      ...fullTimeMaxbetReferCol,
      ...groupCols,
      ...lastUpdatedFixedCol
    ] as Array<Column>;
  };

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
