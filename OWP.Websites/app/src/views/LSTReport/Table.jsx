import { Component } from 'react';
import ReactTable from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns';
import FilterHeader from './FilterHeader';
import { PageSizes, DefaultPageSize } from '../Shared/Constants';
import ReportConst from './constant/ReportConst';
import PropTypes from 'prop-types';
import OrderHeader from './OrderHeader';
import { isEqual } from 'lodash';
import { v4 as uuidv4 } from 'uuid';
import OrderHelper from './Helper/OrderHelper';
import NormalizeDataHelper from './Helper/NormalizeDataHelper';

const ReactTableFixedColumns = withFixedColumns(ReactTable);

const propTypes = {
  columnSelected: PropTypes.array.isRequired,
  data: PropTypes.array.isRequired,
  columnFilter: PropTypes.array.isRequired,
  onClear: PropTypes.func.isRequired,
  onFilter: PropTypes.func.isRequired,
  columnOrder: PropTypes.object.isRequired,
  onOrder: PropTypes.func.isRequired
};

class Table extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedColumn: [],
      orderInfo: {
        orderStrategy: '',
        subColumId: ''
      }
    };
    this.renderSelectedColumn = this.renderSelectedColumn.bind(this);
    this.renderFixedColumn = this.renderFixedColumn.bind(this);
    this.renderSubColumn = this.renderSubColumn.bind(this);
    this.renderValueSubColumn = this.renderValueSubColumn.bind(this);
    this.handleOrderMatch = this.handleOrderMatch.bind(this);
    this.getKeyword = this.getKeyword.bind(this);
    this.getOrderName = this.getOrderName.bind(this);
    this.renderCellStyleTurnOver = this.renderCellStyleTurnOver.bind(this);
  }

  getKeyword = name => {
    const { columnFilter } = this.props;
    const item = columnFilter.find(column => isEqual(column.columnName, name));
    return item ? item.keyword : '';
  };

  handleOrderMatch = orderStrategy => {
    const { data } = this.state;
    const orderData = isEqual(orderStrategy, ReportConst.ORDER_ASC)
      ? OrderHelper.orderMatchAsc(data)
      : OrderHelper.orderMatchDesc(data);
    const orderInfo = {
      orderStrategy,
      subColumId: ''
    };
    this.setState({ data: orderData, orderInfo });
  };

  renderValueSubColumn = info => {
    const { column } = info;
    const subColumnId = column.id;
    const parentColumnId = column.parentColumn.id;
    const originalInfo = info.original;
    const parentColumn = originalInfo[parentColumnId];
    let value = parentColumn[subColumnId];
    if (value === '-') {
      return <div className='text-right'>{value}</div>;
    }
    value = NormalizeDataHelper.formatNumberWithTwoDecimal(value);
    const isNegative = Number.parseFloat(value) < 0;
    return (
      <div className='text-right' style={{ color: isNegative ? 'red' : '' }}>
        {NormalizeDataHelper.numberWithCommas(value)}
      </div>
    );
  };

  renderFixedColumn = () => {
    const { trader, section, match } = ReportConst;
    const { columnOrder } = this.props;
    const columnFixed = [
      {
        fixed: 'left',
        columns: [
          {
            Header: (
              <FilterHeader
                key={uuidv4()}
                columnName='Trader'
                keyword={this.getKeyword('Trader')}
                title={trader}
                onClear={this.props.onClear}
                onSubmit={this.props.onFilter}
              />
            ),
            accessor: 'Trader',
            width: 120,
            cell: info => <span>{info.original[trader]}</span>
          },
          {
            Header: (
              <FilterHeader
                key={uuidv4()}
                columnName='Section'
                keyword={this.getKeyword('Section')}
                title={section}
                onClear={this.props.onClear}
                onSubmit={this.props.onFilter}
              />
            ),
            accessor: 'Section',
            width: 120,
            cell: info => <span>{info.original[section]}</span>
          }
        ]
      },
      {
        Header: (
          <OrderHeader
            key={uuidv4()}
            name={match}
            subColId=''
            orderName={this.getOrderName(columnOrder, match)}
            onOrder={(subColumnId, orderName) =>
              this.props.onOrder(match, subColumnId, orderName)
            }
          />
        ),
        accessor: 'Match',
        width: 90,
        cell: info => <div className='text-right'>{info.original[match]}</div>
      }
    ];
    return columnFixed;
  };

  renderCellStyleTurnOver(_, rowInfo, column) {
    if (
      rowInfo &&
      column &&
      column.parentColumn &&
      column.parentColumn.accessor === 'Turnover'
    ) {
      return {
        style: {
          background: '#fbf3f3'
        }
      };
    } else {
      return {};
    }
  }

  renderSubColumn = (subColumn, columnId) => {
    const { columnOrder } = this.props;
    return subColumn.map(subCol => {
      return {
        Header: (
          <OrderHeader
            name={subCol.name}
            color={subCol.color}
            subColId={subCol.id}
            orderName={this.getOrderName(columnOrder, subCol.id, columnId)}
            onOrder={(subColumId, orderName) =>
              this.props.onOrder(columnId, subColumId, orderName)
            }
          />
        ),
        accessor: subCol.name,
        width: 100,
        id: subCol.id,
        Cell: this.renderValueSubColumn,
        getProps: this.renderCellStyleTurnOver
      };
    });
  };

  getOrderName = (columnOrder, subColumnId, columnId) => {
    if (
      isEqual(subColumnId, columnOrder.columnId) ||
      (isEqual(subColumnId, columnOrder.subColumnId) &&
        isEqual(columnOrder.columnId, columnId))
    ) {
      return columnOrder.orderName;
    } else {
      return '';
    }
  };

  renderSelectedColumn = selectedColumn => {
    const columnSelected = selectedColumn.map(column => {
      return {
        Header: column.name,
        accessor: column.name,
        id: column.id,
        columns: this.renderSubColumn(column.subColumn, column.id)
      };
    });
    const columnFixed = this.renderFixedColumn();
    return [...columnFixed, ...columnSelected];
  };

  render() {
    const { columnSelected } = this.props;
    const { data } = this.props;
    return (
      <div className='table-content'>
        <ReactTableFixedColumns
          data={data}
          noDataText='There is no data.'
          columns={this.renderSelectedColumn(columnSelected)}
          resizable={false}
          sortable={false}
          filterable={false}
          pageSizeOptions={PageSizes}
          defaultPageSize={DefaultPageSize}
          minRows={3}
          className='-striped'
          showPagination={false}
        />
        <span className='total-records'>Total records: {data.length}</span>
      </div>
    );
  }
}
Table.propTypes = propTypes;
export default Table;
