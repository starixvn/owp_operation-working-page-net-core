import PropTypes from 'prop-types';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import changeLogColumn from './changeLogColumn';
import cellLogFormatSelector from './cellLogFormatSelector';
const ReactTableFixedColumns = withFixedColumns(ReactTable);

const propTypes = {
  logs: PropTypes.array.isRequired
};

const ChangeLogTable = ({ logs }) => {
  const renderValueColumn = info => {
    const { column } = info;
    const columnId = column.id;
    const originalInfo = info.original;
    const value = originalInfo[columnId];
    return cellLogFormatSelector(columnId, value, originalInfo.Action);
  };

  const renderSelectedColumn = columns =>
    columns.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        id: column.id,
        width: column.width,
        headerStyle: column.headerStyle,
        Cell: renderValueColumn
      };
    });
  if (!logs.length) {
    return <span className='no-data'>There is no data</span>;
  }
  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={logs}
        columns={renderSelectedColumn(changeLogColumn.Columns)}
        resizable={false}
        sortable={false}
        filterable={false}
        pageSize={logs.length}
        minRows={3}
        className='-striped'
        showPagination={false}
      />
    </div>
  );
};
ChangeLogTable.propTypes = propTypes;
export default ChangeLogTable;
