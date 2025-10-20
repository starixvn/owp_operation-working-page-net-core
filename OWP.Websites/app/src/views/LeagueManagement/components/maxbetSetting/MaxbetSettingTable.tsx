import { isEqual } from 'lodash';
import { HistoryIcon } from 'mdi-react';
import { MouseEvent, useEffect } from 'react';
import ReactTable, { CellInfo, Column } from 'react-table';
import withFixedColumns from 'react-table-hoc-fixed-columns-ts';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { editPopupType, maxbetSettingTable } from './Const.json';
import { formatMaxbetValue, getGroupColumnId, isColumnKey } from './helper/helper';
import useStore, { RootState } from './store/storeNative';
import { ColumnModel, SubColumnModel } from './typeNative';

const ReactTableFixedColumns = withFixedColumns(ReactTable);
const {
  key,
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

const MaxbetSettingTable = () => {
  const {
    getMaxbetSettings,
    getSecondAcceptValues,
    maxbetSettings,
    loading,
    policy,
    setEditModel,
    setLogData,
    setOpenLogPopup,
    setOpenEditPopup,
    setPopupType,
    editModel,
  } = useStore<RootState>(state => state);

  useEffect(() => {
    getMaxbetSettings();
    getSecondAcceptValues();
  }, []);

  const handleEditPopup = (column: string, columnKey: number, oldValue: number | string, popupType: number) => {
    if (!policy.HasWrite) {
      return;
    }
    if (!isColumnKey(column)) {
      setEditModel({ column, fTHDP: columnKey, oldValue });
      setPopupType(popupType);
      setOpenEditPopup();
    }
  };

  const handleOpenPopup = (maxBetValue: number) => {
    setLogData(maxBetValue);
    setOpenLogPopup();
  };

  const renderValue = (
    value: number | string,
    id: string,
    maxBetValue: number
  ) => {
    const ACTIVE_CLICK = ' activeClick';
    const renderCellValue = () => {
      switch (getGroupColumnId(id)) {
        case lastUpdate.id:
          return <>
            <span className='username'>{value}</span>
            <div onClick={() => handleOpenPopup(maxBetValue)}>
              <HistoryIcon size={16} />
            </div>
          </>;
        case secondAcceptValue.id:
          return <div
            className='cell-test'
            style={{ width: '100%', height: '100%' }}
            onDoubleClick={() => handleEditPopup(id, maxBetValue, value, editPopupType.secondAccept)}>
            {value}
          </div>;
        case 'spread':
          return value ?
            <div
              className='cell-test'
              style={{ width: '100%', height: '100%' }}
            >
              {((value as number) * 100).toLocaleString()}%
            </div>
            : null;
        default:
          return <div
            className='cell-test-2'
            style={{ width: '100%', height: '100%' }}
            onDoubleClick={() => handleEditPopup(id, maxBetValue, value, editPopupType.maxbet)}>
            {formatMaxbetValue(value as number)}
          </div>;
      }
    };

    const handleClick = (e: MouseEvent<HTMLDivElement>) => {
      const list: HTMLCollectionOf<Element> = document.getElementsByClassName(ACTIVE_CLICK);
      for (const element of Array.from(list)) {
        element.className = element.className.replace(ACTIVE_CLICK, '');
      }
      e.currentTarget.className += ACTIVE_CLICK;
    };

    const getActiveClassName = maxBetValue === editModel.fTHDP && id === editModel.column
      ? ACTIVE_CLICK
      : '';

    return (
      <div
        className={`${id} cell${getActiveClassName}`}
        onClick={handleClick}
        style={{ userSelect: 'none' }}>
        {renderCellValue()}
      </div>
    );
  };

  const renderFixedColumns = (column: ColumnModel) => {
    const renderFixedColumn = (subCol: SubColumnModel) => {
      return {
        Header: subCol.name,
        headerClassName: `${subCol.id}-header`,
        id: `${subCol.id}`,
        accessor: `${subCol.id}`,
        className: `${subCol.id}-value`,
        Cell: (cellInfo: CellInfo) =>
          renderValue(
            cellInfo.original[subCol.id],
            subCol.id,
            cellInfo.row.fT_HDP
          )
      };
    };

    return [
      {
        Header: column.name,
        accessor: '',
        headerClassName: `${column.id}-group-header`,
        fixed: column.fixed,
        id: column.id,
        columns: [renderFixedColumn(column.subColumn[0])]
      }
    ];
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
          Cell: (cellInfo: CellInfo) => renderValue(
            cellInfo.original[`${subCol.id}`],
            subCol.id,
            cellInfo.row.fT_HDP
          )
        };
      });
    };

    return groupColumns.map(column => {
      return {
        Header: column.name,
        headerClassName: `${column.id}-group-header`,
        accessor: column.name,
        id: column.id,
        columns: renderSubColumn(
          column.subColumn,
          column.subColWidth,
          column.headerClassName
        )
      };
    });
  };

  const renderColumn = () => {
    const keyFixedCol = renderFixedColumns(key);
    const groupCols = renderGroupColumn();
    const lastUpdatedFixedCol = renderFixedColumns(lastUpdate);
    return [...keyFixedCol, ...groupCols, ...lastUpdatedFixedCol] as Array<Column>;
  };

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      {maxbetSettings && (
        <div className='maxbet-setting-table'>
          <ReactTableFixedColumns
            data={maxbetSettings}
            noDataText='There is no data.'
            columns={renderColumn()}
            resizable={false}
            sortable={false}
            filterable={false}
            pageSize={maxbetSettings.length}
            minRows={3}
            className='-highlight -striped'
            showPagination={false}
          />
          <div className='total-records'>
            Total records: {maxbetSettings.length}
          </div>
        </div>
      )}
    </>
  );
};

export default MaxbetSettingTable;
