import PropTypes from 'prop-types';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import FilterHeader from './FilterHeader';
import OrderHeader from './OrderHeader';
import { TableConst } from '../../constants/reportConst';
import { isEqual } from 'lodash';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

const propTypes = {
  columnSelected: PropTypes.array.isRequired,
  data: PropTypes.array.isRequired
};
const ReportTable = props => {
  const { columnSelected, data, ...rest } = props;
  const { la, section, totalmatches } = TableConst.fixedColumnLA;

  const renderValueSubColumn = info => {
    const { column } = info;
    const subColumnId = column.id;
    const originalInfo = info.original;
    const value = originalInfo[subColumnId];
    if (subColumnId !== la.columnId && subColumnId !== section.columnId) {
      return <div className='text-right'>{value || '-'}</div>;
    }
    return <div>{value || '-'}</div>;
  };

  const renderSubcolumn = subColumn => {
    return subColumn.map(subCol => {
      const headerModel = {
        columnName: subCol.name,
        columnId: subCol.id,
        rest
      };
      return {
        Header: <OrderHeader {...headerModel} />,
        accessor: subCol.id,
        width: 115,
        id: subCol.id,
        Cell: renderValueSubColumn
      };
    });
  };

  const renderFixedColumn = () => {
    const headerModel = {
      columnName: totalmatches.columnName,
      columnId: totalmatches.columnId,
      rest
    };
    const columnFixed = [
      {
        fixed: 'left',
        columns: [
          {
            Header: (
              <FilterHeader columnId={la.columnId} columnName={la.columnName} />
            ),
            accessor: `${la.columnId}`,
            width: 120,
            id: `${la.columnId}`,
            Cell: renderValueSubColumn
          },
          {
            Header: (
              <FilterHeader
                columnId={section.columnId}
                columnName={section.columnName}
              />
            ),
            accessor: `${section.columnId}`,
            width: 120,
            id: `${section.columnId}`,
            Cell: renderValueSubColumn
          }
        ]
      },
      {
        Header: <OrderHeader {...headerModel} />,
        accessor: `${totalmatches.columnId}`,
        width: 110,
        id: `${totalmatches.columnId}`,
        Cell: renderValueSubColumn
      }
    ];
    return columnFixed;
  };

  const renderSelectedColumn = selectedColumn => {
    const newColumnSelected = selectedColumn.map(column => {
      return {
        Header:
          isEqual(column.name, 'OTHERS') || isEqual(column.name, '5ML')
            ? ''
            : column.name,
        accessor: column.name,
        id: column.id,
        columns: renderSubcolumn(column.subColumn)
      };
    });
    const columnFixed = renderFixedColumn();
    return [...columnFixed, ...newColumnSelected];
  };
  return (
    <div className='table-content'>
      <ReactTableFixedColumns
        data={data}
        noDataText='There is no data.'
        columns={renderSelectedColumn(columnSelected)}
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

ReportTable.propTypes = propTypes;
export default ReportTable;
