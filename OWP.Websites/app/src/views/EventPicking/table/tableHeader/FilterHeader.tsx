import {
  clearFilter,
  filterBySearchKey,
  setColumnKeyValue,
  setValueFilter,
  clearSingleColumnFilter
} from '../eventPickingTableSlice/reducer';
import { useAppDispatch } from '../../app/hooks';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import FilterHeaderCommon from '../../../Shared/tableCommon/FilterHeaderCommon';
import { TFilterState } from '../../../Shared/tableCommon/typeCommon';
import { TColumnKey } from '../../types/dataTableType';

type FilterHeaderProps = {
  columnKey: string;
};

const FilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.eventPickingTable);
  const hasReadPermission = useSelector(
    (state: RootState) => state.turnOverMarginPolicy.permission.HasRead
  );

  const handleClickFilterCallback = (value: string) => {
    dispatch(setColumnKeyValue(value));
  };
  const handleClearFilterCallback = () => dispatch(clearFilter());
  const handleFilterBySearchKeyCallback = (filterState: TFilterState) =>
    dispatch(filterBySearchKey({ filterState, hasReadPermission }));
  const handleValueFilterCallback = (columnHaveBeenFiltered: TFilterState) =>
    dispatch(setValueFilter(columnHaveBeenFiltered));

  const handleClearSingleColumnFilter = () => {
    const columnKeyTyped = columnKey as TColumnKey;
    dispatch(
      clearSingleColumnFilter({
        columnKey: columnKeyTyped,
        hasReadPermission
      })
    );
  };

  return (
    <FilterHeaderCommon
      isClearSingleColumnFilter
      onClearSingleColumnFilter={handleClearSingleColumnFilter}
      columnKey={columnKey}
      model={model}
      handleClickFilterCallback={handleClickFilterCallback}
      handleClearFilterCallback={handleClearFilterCallback}
      handleFilterBySearchKeyCallback={handleFilterBySearchKeyCallback}
      handleValueFilterCallback={handleValueFilterCallback}
    />
  );
};

export default FilterHeader;
