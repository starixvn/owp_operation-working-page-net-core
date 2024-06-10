import { clearFilter, setMultipleFilter } from './leagueInfoTableSlice/reducer';
import { useAppDispatch } from '../app/hooks';
import MultipleFilterHeaderCommon from '../../../../Shared/tableCommon/MultipleFilterHeader';
import store from '../app/storeNative';
import { TColumnKey } from './TableType';
import helper from './leagueInfoTableSlice/calculateFilterOptions';

type FilterHeaderProps = {
  columnKey: TColumnKey;
};

const FilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();

  const getFilterInfo = () =>
    store.getState().leagueInfoTableSlice.multipleFilter;
  const { entitiesOriginal, multipleFilter } =
    store.getState().leagueInfoTableSlice;
  const options = helper[columnKey](entitiesOriginal);
  return (
    <MultipleFilterHeaderCommon
      setOptions={() => helper[columnKey](entitiesOriginal)}
      setSelected={() => getFilterInfo()[columnKey].selected}
      isFilter={multipleFilter[columnKey].isFilter}
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
    />
  );
};

export default FilterHeader;
