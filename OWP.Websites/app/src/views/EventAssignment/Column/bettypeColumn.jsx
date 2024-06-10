import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../Shared/FilterHeader/FilterHeaderNative';
import BettypeCell from '../Cell/BettypeCell';
import {
  BettypeColumnWidth,
  ColumnTV,
  ColumnProvider
} from '../constants/Column';
import { getKeyword } from '../FilterHelper';
import { v4 as uuidv4 } from 'uuid';
import isEqual from 'lodash.isequal';

const renderBettypeColumn = (
  bettypeColumns,
  dataSource,
  policy,
  filterModel,
  action,
  closePopup
) => {
  const { onClickFilter, onClearTextFilter, columnFilters } = filterModel;
  const { onSavedAssigment, onDeletedAssigment } = action;
  const columns = bettypeColumns.filter(
    b =>
      !isEqual(b.value, ColumnTV.value) &&
      !isEqual(b.value, ColumnProvider.value)
  );
  return columns.map(col => {
    return (
      <Column
        key={uuidv4()}
        columnKey={col.value}
        header={
          <FilterHeader
            key={closePopup ? uuidv4() : col.value}
            name={col.value}
            title={col.label}
            keyword={getKeyword(columnFilters, col.value)}
            onClear={() => onClearTextFilter([])}
            onSubmit={(name, value) => onClickFilter(name, value)}
          />
        }
        width={BettypeColumnWidth}
        cell={({ rowIndex }) => (
          <BettypeCell
            policy={policy}
            bettypeName={col.value}
            rowIndex={rowIndex}
            data={dataSource}
            onSavedAssigment={onSavedAssigment}
            onDeletedAssigment={onDeletedAssigment}
          />
        )}
      />
    );
  });
};

export { renderBettypeColumn };
