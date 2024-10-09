import {
  clearFilter,
  setMultipleFilter
} from '../eventPickingTableSlice/reducer';
import { useAppDispatch } from '../../app/hooks';
import MultipleFilterHeaderCommon from '../../../Shared/tableCommon/MultipleFilterHeader';
import store from '../../app/store';
import { TColumnKey } from '../../types/dataTableType';

type FilterHeaderProps = {
  columnKey: TColumnKey;
};

const MultipleFilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();

  const getFilterInfo = () => store.getState().eventPickingTable.multipleFilter;
  return (
    <MultipleFilterHeaderCommon
      setOptions={() => getFilterInfo()[columnKey].options}
      setSelected={() => getFilterInfo()[columnKey].selected}
      setIsFilter={() => getFilterInfo()[columnKey].isFilter}
      onFilter={selected =>
        dispatch(
          setMultipleFilter({
            columnKey,
            selected
          })
        )
      }
      onClear={() => dispatch(clearFilter())}
    />
  );
};

export default MultipleFilterHeader;
