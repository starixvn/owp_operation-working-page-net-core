import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import OrderHeader from './OrderHeader';
import { prematchTable } from '../../Const.json';
import { ColumnProp } from '../../typeNative';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

const {
  traderCol,
  totalMatchesHdpOuCol,
  totalLeagueHdpOuCol,
  totalLeagueHDP,
  totalLeagueOU,
  totalMatchesOU,
  totalMatchesHDP,
  subColumn
} = prematchTable;
const freezeColumns = [traderCol];
const freezeOrderColumns = [totalLeagueHdpOuCol, totalMatchesHdpOuCol];
const groupColumns = [
  totalLeagueHDP,
  totalLeagueOU,
  totalMatchesHDP,
  totalMatchesOU
];

const ReportTable = () => {
  const reportData = useSelector((state: RootState) => state.report.reportData);
  const isOpenTable = useSelector(
    (state: RootState) => state.report.isOpenTable
  );
  const status = useSelector((state: RootState) => state.report.status);

  const renderFixedColumn = () => {
    return freezeColumns.map(column => {
      return {
        fixed: column.fixed,
        Header: <span>{column.name}</span>,
        headerClassName: `${column.id}-header`,
        width: column.width,
        id: `${column.id}`,
        accessor: `${column.id}`,
        className: `${column.id}-value`,
        Cell: (cellInfo: CellInfo) => cellInfo.original[column.id] ?? '-'
      };
    });
  };

  const renderFixedOrderColumns = () => {
    return freezeOrderColumns.map(column => {
      return {
        Header: (
          <OrderHeader
            columnId={column.id}
            subColName={column.name}
            subColId={column.id}
          />
        ),
        headerClassName: `${column.id}-header`,
        width: column.width,
        id: `${column.id}`,
        accessor: `${column.id}`,
        className: `${column.id}-value`,
        Cell: (cellInfo: CellInfo) => cellInfo.original[column.id] ?? '-'
      };
    });
  };

  const renderFixedColumns = () => {
    return [
      {
        Header: '',
        accessor: '',
        fixed: 'left',
        id: '',
        columns: [...renderFixedColumn(), ...renderFixedOrderColumns()]
      }
    ];
  };

  const renderGroupColumn = () =>
    groupColumns.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        id: column.id,
        columns: renderSubColumn(
          [...subColumn, column.specialColumn],
          column.id
        )
      };
    });

  const renderSubColumn = (subColumns: ColumnProp[], groupId: string) => {
    return subColumns.map(subCol => {
      return {
        Header: (
          <OrderHeader
            columnId={groupId}
            subColName={subCol.name}
            subColId={`${groupId}${subCol.id}`}
          />
        ),
        accessor: `${groupId}${subCol.id}`,
        width: 70,
        id: `${groupId}${subCol.id}`,
        Cell: (cellInfo: CellInfo) =>
          cellInfo.original[`${groupId}${subCol.id}`] ?? '-'
      };
    });
  };

  const renderColumn = () => {
    const fixed = renderFixedColumns();
    const groupCols = renderGroupColumn();
    return [...fixed, ...groupCols] as Array<Column>;
  };
  if (isEqual(status, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      {isOpenTable && (
        <div className='table-content'>
          <ReactTableFixedColumns
            data={reportData}
            noDataText='There is no data.'
            columns={renderColumn()}
            resizable={false}
            sortable={false}
            filterable={false}
            pageSize={reportData.length}
            minRows={3}
            className='-striped'
            showPagination={false}
          />
          <span className='total-records'>
            Total records: {reportData.length}
          </span>
        </div>
      )}
    </>
  );
};

export default ReportTable;
