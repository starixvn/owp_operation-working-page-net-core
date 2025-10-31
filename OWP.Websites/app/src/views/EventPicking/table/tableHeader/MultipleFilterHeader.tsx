import {
  clearFilter,
  setMultipleFilter,
  clearSingleColumnFilter
} from '../eventPickingTableSlice/reducer';
import { useAppDispatch } from '../../app/hooks';
import MultipleFilterHeaderCommon from '../../../Shared/tableCommon/MultipleFilterHeader';
import store, { RootState } from '../../app/store';
import { TColumnKey } from '../../types/dataTableType';
import { useSelector } from 'react-redux';

type FilterHeaderProps = {
  columnKey: TColumnKey;
};

const MultipleFilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const hasReadPermission = useSelector(
    (state: RootState) => state.turnOverMarginPolicy.permission.HasRead
  );
  const model = useSelector((state: RootState) => state.eventPickingTable);

  const getFilterInfo = () => store.getState().eventPickingTable.multipleFilter;

  const handleClearSingleColumnFilter = () => {
    // Check if this column has isFilter = true and if it's the only one
    const { multipleFilter, filterState } = model;
    const columnIsFiltered = multipleFilter[columnKey].isFilter;

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
        dispatch(clearSingleColumnFilter({ columnKey, hasReadPermission }));
      }
    } else {
      dispatch(clearSingleColumnFilter({ columnKey, hasReadPermission }));
    }
  };

  return (
    <MultipleFilterHeaderCommon
      columnKey={columnKey}
      isClearSingleColumnFilter
      setOptions={() => getFilterInfo()[columnKey].options}
      setSelected={() => getFilterInfo()[columnKey].selected}
      setIsFilter={() => getFilterInfo()[columnKey].isFilter}
      onClearSingleColumnFilter={handleClearSingleColumnFilter}
      onFilter={selected =>
        dispatch(
          setMultipleFilter({
            columnKey,
            selected,
            hasReadPermission
          })
        )
      }
      onClear={() => dispatch(clearFilter())}
    />
  );
};

export default MultipleFilterHeader;
