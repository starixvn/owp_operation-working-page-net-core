import PropTypes from 'prop-types';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import DeletedLeagueColumn from './DeletedLeagueColumn';
import RecoverCell from './RecoverCell';
import { formatTime } from '../../ImportPrematchHelperNative';

const ReactTableFixedColumns = withFixedColumns(ReactTable);
const propTypes = {
  leagueDeleted: PropTypes.array.isRequired
};

const DeletedLeagueTable = props => {
  const { leagueDeleted } = props;

  const renderValueColumn = info => {
    const { column } = info;
    const columnId = column.id;
    const originalInfo = info.original;
    const value = originalInfo[columnId];
    switch (columnId) {
      case DeletedLeagueColumn.Columns[0].id:
        return info.index + 1;
      case DeletedLeagueColumn.Columns[3].id:
        return formatTime(
          value,
          'minute',
          'MM/DD/YYYY h:mm A'
        );
      case DeletedLeagueColumn.Columns[4].id:
        return <RecoverCell {...originalInfo} />;
      default:
        return value;
    }
  };

  const renderSelectedColumn = columns =>
    columns.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        width: column.width,
        id: column.id,
        headerStyle: column.headerStyle,
        index: column.index,
        Cell: renderValueColumn
      };
    });
  if (!leagueDeleted.length) {
    return <span className='no-data'>There is no data</span>;
  }
  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={leagueDeleted}
        noDataText='There is no data.'
        columns={renderSelectedColumn(DeletedLeagueColumn.Columns)}
        resizable={false}
        sortable={false}
        filterable={false}
        pageSize={leagueDeleted.length}
        minRows={1}
        className='-striped'
        showPagination={false}
      />
    </div>
  );
};

DeletedLeagueTable.propTypes = propTypes;
export default DeletedLeagueTable;
