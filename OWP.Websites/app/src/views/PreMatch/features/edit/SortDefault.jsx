import { useStableDispatch } from '../../ImportPrematchHelperNative';
import { sortByDefault } from '../league/shiftSlice/shiftSlice';
import { resetCopyInfo } from '../copy/copySlice';

const SortDefault = () => {
  const dispatch = useStableDispatch();
  const handleSort = () => {
    dispatch(sortByDefault());
    dispatch(resetCopyInfo());
  };
  return (
    <button className='button-sort' onClick={handleSort}>
      Sort
    </button>
  );
};

export default SortDefault;
