import { getEventPickings } from '../table/eventPickingTableSlice/eventSliceThunkNative';
import store from '../app/store';
import { useAppDispatch } from '../app/hooks';

const Command = () => {
  const dispatch = useAppDispatch();

  const handleSearch = () => {
    const { param } = store.getState().filter;
    dispatch(getEventPickings(param));
  };

  return (
    <button className='button-submit' onClick={handleSearch}>
      Search
    </button>
  );
};

export default Command;
