import {
  clearFilter,
  filterBySearchKey,
  setColumnKeyValue,
  setValueFilter
} from '../tableSlice/tableSlice';
import { useAppDispatch } from '../../app/hookNative';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { TFilterState } from '../../../Shared/tableCommon/typeCommon';
import FilterHeaderCommon from '../../../Shared/tableCommon/FilterHeaderCommon';

type FilterHeaderProps = {
  columnKey: string;
};

const FilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.events);

  const handleClickFilterCallback = (value: string) => {
    dispatch(setColumnKeyValue(value));
  };
  const handleClearFilterCallback = () => dispatch(clearFilter());
  const handleFilterBySearchKeyCallback = (filterState: TFilterState) =>
    dispatch(filterBySearchKey(filterState));
  const handleValueFilterCallback = (columnHaveBeenFiltered: TFilterState) =>
    dispatch(setValueFilter(columnHaveBeenFiltered));

  return (
    <>
      <FilterHeaderCommon
        columnKey={columnKey}
        model={model}
        handleClickFilterCallback={handleClickFilterCallback}
        handleClearFilterCallback={handleClearFilterCallback}
        handleFilterBySearchKeyCallback={handleFilterBySearchKeyCallback}
        handleValueFilterCallback={handleValueFilterCallback}
      />
    </>
  );
};

export default FilterHeader;
