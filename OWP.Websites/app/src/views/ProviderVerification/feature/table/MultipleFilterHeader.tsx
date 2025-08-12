import MultipleFilterHeaderCommon from '../../../Shared/tableCommon/MultipleFilterHeader';
import { useAppDispatch } from '../../app/hookNative';
import store from '../../app/storeNative';
import { TColumnKey } from '../../type/table';
import helper from './calculateFilterOption';
import { setMultipleFilter, handleClearFilter } from './tableSlice';

const MultipleFilterHeader = ({ columnKey }: { columnKey: TColumnKey }) => {
  const dispatch = useAppDispatch();

  const getFilterInfo = () => store.getState().events.multipleFilter;

  const { dataSourceOriginal, multipleFilter } = store.getState().events;
  const options = helper[columnKey](dataSourceOriginal);

  return (
    <MultipleFilterHeaderCommon
      setOptions={() => helper[columnKey](dataSourceOriginal)}
      selected={getFilterInfo()[columnKey].selected}
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
      onClear={() => dispatch(handleClearFilter())}
    />
  );
};

export default MultipleFilterHeader;
