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
    dispatch(clearSingleColumnFilter({ columnKey, hasReadPermission }));
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
