import { useAppDispatch, useAppSelector } from '../app/hooks';
import { handleShowUnreadMessage } from '../table/eventAssignmentTableSlice';

const ShowUnreadMessage = () => {
  const dispatch = useAppDispatch();
  const showUnreadMessage = useAppSelector(
    state => state.eventAssignmentTable.isShowUnreadMsg
  );

  const handleOnChange = () => {
    dispatch(handleShowUnreadMessage());
  };

  return (
    <div className='filter-control-checkbox'>
      <label>
        <input
          className='unread-checkbox'
          type='checkbox'
          checked={showUnreadMessage}
          onChange={handleOnChange}
        />{' '}
        Show Unread Message Only
      </label>
    </div>
  );
};
export default ShowUnreadMessage;
