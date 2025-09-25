import {
  clearFilter,
  setMultipleFilter
} from '../eventPickingTableSlice/reducer';
import { useAppDispatch } from '../../app/hooks';
import MultipleFilterHeaderCommon from '../../../Shared/tableCommon/MultipleFilterHeader';
import store from '../../app/store';
import { TColumnKey } from '../../types/dataTableType';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';

type FilterHeaderProps = {
  columnKey: TColumnKey;
};

const MultipleFilterHeader = ({ columnKey }: FilterHeaderProps) => {
  const dispatch = useAppDispatch();
  const hasReadPermission = useSelector((state: RootState) => state.turnOverMarginPolicy.permission.HasRead);

  const getFilterInfo = () => store.getState().eventPickingTable.multipleFilter;
  return (
    <MultipleFilterHeaderCommon
      columnKey={columnKey}
      setOptions={() => getFilterInfo()[columnKey].options}
      setSelected={() => getFilterInfo()[columnKey].selected}
      setIsFilter={() => getFilterInfo()[columnKey].isFilter}
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
