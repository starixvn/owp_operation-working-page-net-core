import {
  clearFilter,
  clearSingleColumnFilter,
  filterBySearchKey,
  setColumnKeyValue,
  setValueFilter
} from './leagueInfoTableSlice/reducer';
import { useAppDispatch } from '../app/hooks';
import { useSelector } from 'react-redux';
import { RootState } from '../app/storeNative';
import FilterHeaderCommon from '../../../../Shared/tableCommon/FilterHeaderCommon';
import { TFilterState } from '../../../../Shared/tableCommon/typeCommon';
import { TColumnHasMultipleFilterKey } from './TableType';

type FilterHeaderProps = {
  columnKey: string;
};

const FilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.leagueInfoTableSlice);

  const handleClickFilterCallback = (value: string) => {
    dispatch(setColumnKeyValue(value));
  };
  const handleClearFilterCallback = () => dispatch(clearFilter());
  const handleFilterBySearchKeyCallback = (filterState: TFilterState) =>
    dispatch(filterBySearchKey(filterState));
  const handleValueFilterCallback = (columnHaveBeenFiltered: TFilterState) =>
    dispatch(setValueFilter(columnHaveBeenFiltered));

  const handleClearSingleColumnFilter = () => {
    // Check if this column has isFilter = true and if it's the only one
    const { multipleFilter, filterState } = model;
    const columnKeyTyped = columnKey as any;
    const columnIsFiltered =
      columnKeyTyped in multipleFilter &&
      multipleFilter[columnKeyTyped as TColumnHasMultipleFilterKey].isFilter;
    const hasTextFilter = (filterState || []).some(
      (filter: TFilterState) => filter.columnKey === columnKey
    );

    if (columnIsFiltered || hasTextFilter) {
      // Count total active filters
      const activeMultipleFilters = Object.values(multipleFilter).filter(
        (filter: any) => filter.isFilter
      ).length;
      const activeTextFilters = filterState.length;

      // If this is the only active filter, clear all filters instead
      if (activeMultipleFilters + activeTextFilters === 1) {
        dispatch(clearFilter());
      } else {
        dispatch(
          clearSingleColumnFilter({
            columnKey: columnKeyTyped
          })
        );
      }
    } else {
      dispatch(
        clearSingleColumnFilter({
          columnKey: columnKeyTyped
        })
      );
    }
  };
  return (
    <FilterHeaderCommon
      columnKey={columnKey}
      model={model}
      isClearSingleColumnFilter
      onClearSingleColumnFilter={handleClearSingleColumnFilter}
      handleClickFilterCallback={handleClickFilterCallback}
      handleClearFilterCallback={handleClearFilterCallback}
      handleFilterBySearchKeyCallback={handleFilterBySearchKeyCallback}
      handleValueFilterCallback={handleValueFilterCallback}
    />
  );
};

export default FilterHeader;
