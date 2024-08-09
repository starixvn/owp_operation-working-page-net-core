import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setAllMatchSelected } from '../eventPickingTableSlice/reducer';
type CheckboxHeaderProps = {
  classNameProps: string | undefined;
  columnKey: string;
};

const CheckAllHeader = ({ classNameProps, columnKey }: CheckboxHeaderProps) => {
  const dispatch = useStableDispatch();
  const isSelectedAll = useSelector(
    (state: RootState) => state.eventPickingTable.isSelectedAll
  );
  const handleOnChange = async event => {
    dispatch(
      setAllMatchSelected({
        isAdded: event?.target?.checked
      })
    );
  };
  return (
    <div className='custom-checkbox'>
      <input
        type='checkbox'
        className='check-by-lst'
        checked={isSelectedAll}
        onClick={handleOnChange}
      />
    </div>
  );
};
export default CheckAllHeader;
