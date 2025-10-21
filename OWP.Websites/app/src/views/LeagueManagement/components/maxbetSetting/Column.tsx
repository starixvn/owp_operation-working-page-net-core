import { maxbetSettingTable } from './Const.json';
import { CellInfo, Column } from 'react-table';
import { ColumnModel, SubColumnModel } from './typeNative';
import Cell from './Cell';
import OrderHeader from '../../../Shared/common/CustomTable/commonCell/OrderCell/OrderableCell';
import store from './store/storeNative';

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

const renderColumn = (orderInfo: { columnId: string, orderType: 'ORDER_DESC' | 'ORDER_ASC' }) => {
  const fullTimeMaxbetReferCol = renderFixedColumns(fullTimeMaxbetRefer,
    <div className='fullTimeMaxbetRefer-header'>
      <OrderHeader
        headerName={fullTimeMaxbetRefer.subColumn[0].name}
        orderType={orderInfo.orderType}
        onOrder={orderType => store.getState().sortMaxbetSettings(fullTimeMaxbetRefer.id, orderType)}
      /></div>);
  const groupCols = renderGroupColumn();
  const lastUpdatedFixedCol = renderFixedColumns(lastUpdate, lastUpdate.name);
  return [
    ...fullTimeMaxbetReferCol,
    ...groupCols,
    ...lastUpdatedFixedCol
  ] as Array<Column>;
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

const renderFixedColumns = (column: ColumnModel, header: React.ReactElement | string) => {
  const subCol = column.subColumn[0];
  return [
    {
      Header: column.name,
      accessor: '',
      headerClassName: `${column.id}-group-header`,
      fixed: column.fixed,
      id: column.id,
      columns: [
        {
          Header: header,
          headerClassName: `${subCol.id}-header`,
          id: `${subCol.id}`,
          accessor: `${subCol.id}`,
          className: `${subCol.id}-value`,
          Cell: (cellInfo: CellInfo) => <Cell
            value={cellInfo.original[subCol.id]}
            id={subCol.id}
            maxBetValue={cellInfo.row.fT_HDP}
          />
        }
      ]
    }
  ];
};

export {
  renderColumn,
  renderGroupColumn,
  renderFixedColumns
};
