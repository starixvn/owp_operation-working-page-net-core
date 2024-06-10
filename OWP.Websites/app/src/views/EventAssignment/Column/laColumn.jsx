import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../Shared/FilterHeader/FilterHeaderNative';
import BettypeCell from '../Cell/BettypeCell';
import {
  LA
} from '../constants/Column';
import { getKeyword } from '../FilterHelper';
import { v4 as uuidv4 } from 'uuid';

const renderLAColumn = (
  dataSource,
  policy,
  filterModel,
  action,
  closePopup
) => {
  const { onClickFilter, onClearTextFilter, columnFilters } = filterModel;
  const { onSavedAssigment, onDeletedAssigment } = action;
  return (
    <Column
      key={LA.columnKey}
      columnKey={LA.columnKey}
      header={
        <FilterHeader
          key={closePopup ? uuidv4() : LA.columnKey}
          name={LA.name}
          title={LA.title}
          keyword={getKeyword(columnFilters, LA.name)}
          onClear={() => onClearTextFilter([])}
          onSubmit={(name, value) => onClickFilter(name, value)}
        />
      }
      width={LA.width}
      cell={({ rowIndex }) => (
        <BettypeCell
          policy={policy}
          bettypeName={LA.columnKey}
          rowIndex={rowIndex}
          data={dataSource}
          onSavedAssigment={onSavedAssigment}
          onDeletedAssigment={onDeletedAssigment}
        />
      )}
    />
  );
};

export {
  renderLAColumn
};
