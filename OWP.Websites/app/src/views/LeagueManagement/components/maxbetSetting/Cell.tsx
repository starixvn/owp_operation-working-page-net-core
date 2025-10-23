import { HistoryIcon } from 'mdi-react';
import { MouseEvent } from 'react';
import {
  formatMaxbetValue,
  formatSpreadValue,
  getGroupColumnId,
  isFullTimeMaxbetReferColumn
} from './helper/helper';
import { editPopupType, maxbetSettingTable } from './Const.json';
import useStore, { RootState } from './store/storeNative';

type TCellProps = {
  value: number | string;
  id: string;
  maxBetValue: number;
};

const { secondAcceptValue, lastUpdate } = maxbetSettingTable;

const Cell = (props: TCellProps) => {
  const { value, id, maxBetValue } = props;
  const ACTIVE_CLICK = ' activeClick';
  const {
    policy,
    setEditModel,
    setLogData,
    setOpenLogPopup,
    setOpenEditPopup,
    setPopupType,
    editModel
  } = useStore<RootState>(state => state);

  const handleOpenPopup = (maxBetValue: number) => {
    setLogData(maxBetValue);
    setOpenLogPopup();
  };

  const handleEditPopup = (
    column: string,
    columnKey: number,
    oldValue: number | string,
    popupType: number
  ) => {
    if (!policy.HasWrite) {
      return;
    }

    if (!isFullTimeMaxbetReferColumn(column)) {
      setEditModel({ column, fTHDP: columnKey, oldValue });
      setPopupType(popupType);
      setOpenEditPopup();
    }
  };

  const renderCellValue = () => {
    const renderCell = (cellProps: {
      onEditPopup: () => void;
      value: number | string | null;
    }) => {
      return (
        <div
          className='cell-test'
          style={{ width: '100%', height: '100%' }}
          onDoubleClick={cellProps.onEditPopup}>
          {cellProps.value}
        </div>
      );
    };

    switch (getGroupColumnId(id)) {
      case lastUpdate.id:
        return (
          <>
            <span className='username'>{value}</span>
            <div onClick={() => handleOpenPopup(maxBetValue)}>
              <HistoryIcon size={16} />
            </div>
          </>
        );
      case secondAcceptValue.id:
        return renderCell({
          onEditPopup: () =>
            handleEditPopup(id, maxBetValue, value, editPopupType.secondAccept),
          value
        });
      case 'spread':
        return renderCell({
          onEditPopup: () =>
            handleEditPopup(id, maxBetValue, value, editPopupType.spread),
          value: formatSpreadValue(value as number)
        });
      default:
        return renderCell({
          onEditPopup: () =>
            handleEditPopup(id, maxBetValue, value, editPopupType.maxbet),
          value: formatMaxbetValue(value as number)
        });
    }
  };

  const handleClick = (e: MouseEvent<HTMLDivElement>) => {
    const list: HTMLCollectionOf<Element> =
      document.getElementsByClassName(ACTIVE_CLICK);
    for (const element of Array.from(list)) {
      element.className = element.className.replace(ACTIVE_CLICK, '');
    }
    e.currentTarget.className += ACTIVE_CLICK;
  };

  const getActiveClassName =
    maxBetValue === editModel.fTHDP && id === editModel.column
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

export default Cell;
