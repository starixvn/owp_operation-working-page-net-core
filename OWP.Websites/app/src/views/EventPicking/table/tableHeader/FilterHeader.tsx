import {
  clearFilter,
  filterBySearchKey,
  setColumnKeyValue,
  setValueFilter
} from '../eventPickingTableSlice/reducer';
import { useAppDispatch } from '../../app/hooks';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import FilterHeaderCommon from '../../../Shared/tableCommon/FilterHeaderCommon';
import { TFilterState } from '../../../Shared/tableCommon/typeCommon';

type FilterHeaderProps = {
  columnKey: string;
};

const FilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.eventPickingTable);

  const handleClickFilterCallback = (value: string) => {
    dispatch(setColumnKeyValue(value));
  };
  const handleClearFilterCallback = () => dispatch(clearFilter());
  const handleFilterBySearchKeyCallback = (filterState: TFilterState) =>
    dispatch(filterBySearchKey(filterState));
  const handleValueFilterCallback = (columnHaveBeenFiltered: TFilterState) =>
    dispatch(setValueFilter(columnHaveBeenFiltered));
  return (
    <FilterHeaderCommon
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
