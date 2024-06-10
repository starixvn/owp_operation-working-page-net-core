import { Dictionary, isEqual } from 'lodash';
import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import { monthReport, precision } from '../../../Const.json';
import { formatFractionDigit } from '../../helpers/helpers';
import { MonthOvertimeData, Overtime } from '../Helper/TypeNative';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

type Props = {
  data: MonthOvertimeData;
};

const { noCol, locationCol, traderCol, totalCol } = monthReport;
const freezeColumns = [noCol, locationCol, traderCol, totalCol];

const ReportTable = (props: Props) => {
  const { totalHours, traderOvertime, dateColumns, totalHoursByDate } =
    props.data;
  const getOvertimeByKey = (overtimeDict: Dictionary<Overtime>, key: string) =>
    overtimeDict[key] || new Overtime();

  const renderDateValue = (info: CellInfo, columnId: string) => {
    const originalInfo = info.original;
    const overtime = originalInfo.overtime;
    const valueByDate = getOvertimeByKey(overtime, columnId);
    const { hour, isOffDay } = valueByDate;

    return (
      <span className={isOffDay ? 'off-day' : ''}>
        {formatFractionDigit(hour, precision)}
      </span>
    );
  };

  const renderFixedColumn = () => {
    return freezeColumns.map(column => {
      const { fixed, name, id, width } = column;
      const renderFooter = () =>
        isEqual(column, totalCol) ? (
          <span className={`${column.id}-footer`}>
            {formatFractionDigit(totalHours, precision)}
          </span>
        ) : (
          ''
        );

      const renderValue = (cellInfo: CellInfo) => {
        const noColValue = cellInfo.index + 1;
        const otherColValue = isEqual(column, totalCol)
          ? formatFractionDigit(cellInfo.original[column.id], precision)
          : cellInfo.original[column.id] ?? '-';

        return isEqual(column, noCol) ? noColValue : otherColValue;
      };

      return {
        fixed,
        Header: name,
        headerClassName: `${id}-header`,
        Footer: renderFooter,
        width,
        id: `${id}`,
        accessor: `${id}`,
        className: `${id}-value`,
        Cell: renderValue
      };
    });
  };

  const renderDateColumns = () => {
    return dateColumns.map((column, index) => {
      const { date: columnId, isWeekend, dayOfWeek } = column;
      const renderFooter = () => {
        const value = totalHoursByDate[index];

        return (
          <span className='date-footer'>
            {formatFractionDigit(value, precision)}
          </span>
        );
      };

      return {
        Header: (
          <>
            <div className='weekday'>{dayOfWeek}</div>
            <div>{columnId}</div>
          </>
        ),
        headerClassName: isWeekend ? 'is-weekend' : '',
        width: 50,
        id: columnId,
        Footer: renderFooter,
        Cell: (cellInfo: CellInfo) => renderDateValue(cellInfo, columnId),
        className: 'date-value'
      };
    });
  };

  const renderColumnsByMonth = () => {
    const fixedColumns = renderFixedColumn();
    const daysColumns = renderDateColumns();

    return [...fixedColumns, ...daysColumns] as Array<Column>;
  };

  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={traderOvertime}
        noDataText='There is no data.'
        columns={renderColumnsByMonth()}
        resizable={false}
        sortable={false}
        filterable={false}
        pageSize={traderOvertime.length}
        minRows={3}
        className='-striped'
        showPagination={false}
      />
      <span className='total-records'>
        Total records: {traderOvertime.length}
      </span>
    </div>
  );
};

export default ReportTable;
