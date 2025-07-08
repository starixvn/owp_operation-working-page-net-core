import {
  clearFilter,
  filterBySearchKey
} from '../tableSlice/tableSlice';
import { useAppDispatch } from '../../app/hookNative';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { TFilterState } from '../../../Shared/tableCommon/typeCommon';
import FilterTradingLink from '../../../Shared/FilterTradingLink';

type FilterHeaderProps = {
  columnKey: string;
  name: string;
  title: string;
};

const FilterHeader = ({ columnKey, name, title }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.events);
  const { filterState, matchesSelected } = model;
  const firstMatchCheckedIsSaba = matchesSelected[0]?.IsSaba;

  return (
    <FilterTradingLink
      firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
      matchesSelected={matchesSelected}
      name={name}
      title={title}
      keyword={filterState.find(x => x.columnKey === columnKey)?.keyword ?? ''}
      onClear={() => dispatch(clearFilter())}
      onSubmit={(columnKey: string, keyword: string) => dispatch(filterBySearchKey({
        columnKey,
        keyword
      }))}
    />
  );
};

export default FilterHeader;
