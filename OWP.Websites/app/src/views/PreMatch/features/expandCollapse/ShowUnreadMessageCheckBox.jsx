import { useSelector } from 'react-redux';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import {
  unShowUnreadMessageOnly,
  showUnreadMessageOnly
} from '../league/shiftSlice/shiftSlice';
import { Input } from 'reactstrap';

const ShowUnreadMessageCheckBox = () => {
  const stableDispatch = useStableDispatch();
  const showUnreadMessageState = useSelector(
    state => state.dayShift.isShowUnreadMessage
  );

  const handelClick = () => {
    showUnreadMessageState
      ? stableDispatch(unShowUnreadMessageOnly())
      : stableDispatch(showUnreadMessageOnly());
  };

  return (
    <div className='checkbox'>
      <Input
        id='prematch-filter-issuse-message'
        type='checkbox'
        defaultChecked={showUnreadMessageState}
        className='form-check-input'
        onClick={handelClick}
      />
      <label
        className='form-check-label'
        htmlFor='prematch-filter-issuse-message'>
        Show unread message only
      </label>
    </div>
  );
};
export default ShowUnreadMessageCheckBox;
