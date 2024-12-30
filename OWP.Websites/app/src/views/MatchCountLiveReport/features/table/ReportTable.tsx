import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import OrderHeader from './OrderHeader';
import { reportTable } from '../../Const.json';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { ColumnProp } from '../../typeNative';
import 'react-table-hoc-fixed-columns-ts/lib/styles.css';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { isEqual } from 'lodash';
import { formatNumber } from '../../helper/helper';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

const { noCol, betType, totalMatches, subColumn } = reportTable;
const freezeColumns = [noCol, betType];
const groupColumns = [totalMatches];

const ReportTable = () => {
  const { reportData, isOpenTable, status } = useSelector(
    (state: RootState) => state.report
  );

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
        Cell: (cellInfo: CellInfo) => cellInfo.original[column.id] ?? ''
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
        columns: [...renderFixedColumn()]
      }
    ];
  };

  const renderGroupColumn = () =>
    groupColumns.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        id: column.id,
        columns: renderSubColumn([...subColumn], column.id)
      };
    });

  const renderSubColumn = (subColumns: ColumnProp[], groupId: string) => {
    return subColumns.map(subCol => {
      return {
        Header: (
          <OrderHeader
            columnId={groupId}
            subColName={subCol.name}
            subColId={subCol.id}
          />
        ),
        accessor: `${subCol.id}`,
        width: 70,
        id: `${subCol.id}`,
        Cell: (cellInfo: CellInfo) =>
          formatNumber(cellInfo.original[`${subCol.id}`]) ?? '-'
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
        </div>
      )}
    </>
  );
};

export default ReportTable;
