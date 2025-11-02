import {
  clearFilter,
  clearSingleColumnFilter,
  setMultipleFilter
} from './leagueInfoTableSlice/reducer';
import { useAppDispatch } from '../app/hooks';
import MultipleFilterHeaderCommon from '../../../../Shared/tableCommon/MultipleFilterHeader';
import store from '../app/storeNative';
import { TColumnHasMultipleFilterKey, TColumnKey } from './TableType';
import helper from './leagueInfoTableSlice/calculateFilterOptions';

type FilterHeaderProps = {
  columnKey: TColumnKey;
};

const MultipleFilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();

  const getFilterInfo = () =>
    store.getState().leagueInfoTableSlice.multipleFilter;
  const { entitiesOriginal } = store.getState().leagueInfoTableSlice;
  const options = helper[columnKey as TColumnHasMultipleFilterKey](entitiesOriginal);

  const handleClearSingleColumnFilter = () => {
    // Check if this column has isFilter = true and if it's the only one
    const { multipleFilter, filterState } =
      store.getState().leagueInfoTableSlice;
    const columnIsFiltered = multipleFilter[columnKey as TColumnHasMultipleFilterKey].isFilter;

    if (columnIsFiltered) {
      // Count total active filters
      const activeMultipleFilters = Object.values(multipleFilter).filter(
        (filter: any) => filter.isFilter
      ).length;
      const activeTextFilters = filterState.length;

      // If this is the only active filter, clear all filters instead
      if (activeMultipleFilters + activeTextFilters === 1) {
        dispatch(clearFilter());
      } else {
        dispatch(clearSingleColumnFilter({ columnKey }));
      }
    } else {
      dispatch(clearSingleColumnFilter({ columnKey }));
    }
  };

  return (
    <MultipleFilterHeaderCommon
      columnKey={columnKey}
      isClearSingleColumnFilter
      setOptions={() => getFilterInfo()[columnKey as TColumnHasMultipleFilterKey].options}
      setSelected={() => getFilterInfo()[columnKey as TColumnHasMultipleFilterKey].selected}
      setIsFilter={() => getFilterInfo()[columnKey as TColumnHasMultipleFilterKey].isFilter}
      onFilter={selected =>
        dispatch(
          setMultipleFilter({
            columnKey,
            selected,
            isFilter: selected.length !== options.length
          })
        )
      }
      onClear={() => dispatch(clearFilter())}
      onClearSingleColumnFilter={handleClearSingleColumnFilter}
    />
  );
};

export default MultipleFilterHeader;
