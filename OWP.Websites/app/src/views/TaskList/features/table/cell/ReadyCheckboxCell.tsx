import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../../../app/storeNative';
import { TUpdateReadyStatus } from '../../../type/TTaskListTable';
import { updateReadyStatusThunk } from '../tableThunk';

const ReadyCheckboxCell = (props: TUpdateReadyStatus) => {
  const { isToday } = useSelector(
    (state: RootState) => state.taskList.refreshModel
  );

  const dispatch = useDispatch();

  const handleChange = () => {
    dispatch(updateReadyStatusThunk(props));
  };

  return (
    <>
      <div className='ready-checkbox'>
        <input
          type='checkbox'
          className='type-row filled-in checkbox'
          onChange={handleChange}
          disabled={!isToday}
        />
      </div>
    </>
  );
};

export default ReadyCheckboxCell;
