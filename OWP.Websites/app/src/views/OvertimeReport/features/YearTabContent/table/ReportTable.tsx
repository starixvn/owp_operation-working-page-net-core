import { isEqual, sum } from 'lodash';
import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import {
  months,
  precision,
  shortMonthLabel,
  yearReport
} from '../../../Const.json';
import { formatFractionDigit } from '../../helpers/helpers';
import { OverTimeDataByYear } from '../../typeNative';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

type PropTypes = {
  data: Array<OverTimeDataByYear>;
};

const { noCol, locationCol, traderCol, totalCol } = yearReport;
const freezeColumns = [noCol, locationCol, traderCol, totalCol];

const ReportTable = (props: PropTypes) => {
  const { data } = props;

  const renderFixedColumns = () => {
    return freezeColumns.map(column => {
      const renderHeader = <span>{column.name}</span>;

      const renderFooter = () =>
        isEqual(column, totalCol) ? (
          <span>
            {formatFractionDigit(sum(data.map(d => d.total)), precision)}
          </span>
        ) : null;

      const renderValue = (cellInfo: CellInfo) => {
        const noColValue = cellInfo.index + 1;
        const otherColValue = isEqual(column, totalCol)
          ? formatFractionDigit(cellInfo.original[column.id], precision)
          : cellInfo.original[column.id] ?? '-';

        return isEqual(column, noCol) ? noColValue : otherColValue;
      };

      return {
        fixed: column.fixed,
        Header: renderHeader,
        headerClassName: `${column.id}-header`,
        Footer: renderFooter,
        width: column.width,
        id: `${column.id}`,
        accessor: `${column.id}`,
        className: `${column.id}-value`,
        Cell: renderValue
      };
    });
  };

  const renderMonthColumns = () => {
    return months.map(column => {
      const renderFooter = () => {
        const overtimeData = data.map(x => x.overtimeData);
        const overtimeHoursByMonth = overtimeData.map(x => x[column.value - 1]);
        const totalOvertimeHoursByMonth = sum(overtimeHoursByMonth);

        return (
          <div className='total-value'>
            {totalOvertimeHoursByMonth
              ? formatFractionDigit(totalOvertimeHoursByMonth, precision)
              : '-'}
          </div>
        );
      };

      const renderOvertimeValue = (info: CellInfo) => {
        const originalInfo = info.original;
        const overtimeData = originalInfo.overtimeData as Array<number | null>;
        const hour = overtimeData[column.value - 1];

        return <span>{hour ? formatFractionDigit(hour, precision) : '-'}</span>;
      };

      return {
        Header: (
          <div className='month'>
            {column.label.slice(0, shortMonthLabel).toUpperCase()}
          </div>
        ),
        width: 100,
        id: column.label,
        Footer: renderFooter,
        footerClassName: 'text-center',
        Cell: (cellInfo: CellInfo) => renderOvertimeValue(cellInfo),
        className: 'date-value'
      };
    });
  };

  const renderColumnsByMonth = () => {
    const dateColumns = renderMonthColumns();
    const fixedColumns = renderFixedColumns();

    return [...fixedColumns, ...dateColumns] as Array<Column>;
  };

  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={data}
        noDataText='There is no data.'
        columns={renderColumnsByMonth()}
        resizable={false}
        sortable={false}
        filterable={false}
        pageSize={data.length}
        minRows={3}
        className='-striped'
        showPagination={false}
      />
      <span className='total-records'>Total records: {data.length}</span>
    </div>
  );
};

export default ReportTable;
