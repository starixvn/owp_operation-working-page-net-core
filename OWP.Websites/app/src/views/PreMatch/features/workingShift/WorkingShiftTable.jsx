import PropTypes from 'prop-types';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import workingShiftColumn from './workingShiftColumn';
const ReactTableFixedColumns = withFixedColumns(ReactTable);

const propTypes = {
  workingShiftData: PropTypes.array.isRequired
};

const WorkingShiftTable = props => {
  const { workingShiftData } = props;
  const renderValueColumn = info => {
    const { column } = info;
    const ColumnId = column.id;
    const originalInfo = info.original;
    const value = originalInfo[ColumnId];
    return <div className='text-left'>{value || '-'}</div>;
  };

  const renderSelectedColumn = columns => {
    const orderableColumn = columns.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        id: column.id,
        width: column.width,
        headerStyle: column.headerStyle,
        Cell: renderValueColumn
      };
    });
    return [...orderableColumn];
  };

  if (!workingShiftData.length) {
    return <span className='no-data'>There is no data</span>;
  }
  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={workingShiftData}
        columns={renderSelectedColumn(workingShiftColumn.Columns)}
        resizable={false}
        sortable={false}
        filterable={false}
        pageSize={workingShiftData.length}
        minRows={3}
        className='-striped'
        showPagination={false}
      />
    </div>
  );
};
WorkingShiftTable.propTypes = propTypes;
export default WorkingShiftTable;
