import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../Shared/FilterHeader/FilterHeaderNative';
import renderCellBase from '../Cell/CellBase';
import {
  MatchNo,
  KickOffTime,
  TV,
  Provider,
  MaxbetSpread,
  Remark
} from '../constants/Column';
import { getKeyword } from '../FilterHelper';
import { v4 as uuidv4 } from 'uuid';

const renderColumnBase = (dataSource, filterModel, columnInfo, closePopup) => {
  const { onClickFilter, onClearTextFilter, columnFilters } = filterModel;
  return (
    <Column
      key={columnInfo.columnKey}
      columnKey={columnInfo.columnKey}
      header={
        <FilterHeader
          key={closePopup ? uuidv4() : columnInfo.columnKey}
          name={columnInfo.name}
          title={columnInfo.title}
          keyword={getKeyword(columnFilters, columnInfo.name)}
          onClear={() => onClearTextFilter([])}
          onSubmit={(name, value) => onClickFilter(name, value)}
        />
      }
      width={columnInfo.width}
      fixed={columnInfo.fixed}
      cell={({ rowIndex }) =>
        renderCellBase(columnInfo.name, dataSource, rowIndex)
      }
    />
  );
};

const renderMatchNoColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, MatchNo, closePopup);

const renderKOTimeColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, KickOffTime, closePopup);

const renderTVColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, TV, closePopup);

const renderProviderColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, Provider, closePopup);

const renderMaxbetSpreadColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, MaxbetSpread, closePopup);

const renderRemarkColumn = (dataSource, filterModel, closePopup) =>
  renderColumnBase(dataSource, filterModel, Remark, closePopup);

export {
  renderMatchNoColumn,
  renderKOTimeColumn,
  renderTVColumn,
  renderProviderColumn,
  renderMaxbetSpreadColumn,
  renderRemarkColumn
};
