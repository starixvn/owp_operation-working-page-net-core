import { getEventPickings } from '../table/eventPickingTableSlice/eventSliceThunkNative';
import store from '../app/store';
import { useAppDispatch } from '../app/hooks';
import { setLastSearchTime } from './slice/filterSlice';

const Command = () => {
  const dispatch = useAppDispatch();

  const handleSearch = () => {
    const { param } = store.getState().filter;
    const now = new Date();
    const gmt8Time = new Date(
      now.toLocaleString('en-US', { timeZone: 'Asia/Taipei' })
    );

    dispatch(setLastSearchTime(gmt8Time));
    dispatch(getEventPickings(param));
  };

  return (
    <button className='button-submit' onClick={handleSearch}>
      Search
    </button>
  );
};

export default Command;
