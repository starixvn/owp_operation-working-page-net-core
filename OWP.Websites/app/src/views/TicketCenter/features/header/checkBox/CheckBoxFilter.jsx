import { useStableDispatch } from '../../../ImportTicketCenterHelperNative';
import { useSelector } from 'react-redux';
import { showUnreadMessageOnly } from '../../Table/eventSlice/eventCheckingSlice';

const CheckBoxFilter = () => {
  const stableDispatch = useStableDispatch();
  const eventChecking = useSelector(state => state.eventChecking);
  const { isShowUnreadMessOnly } = eventChecking;

  const renderShowUnreadMessOnlyCheckBox = () => (
    <>
      <input
        className='unread-checkbox'
        type='checkbox'
        checked={isShowUnreadMessOnly}
        onChange={() =>
          stableDispatch(showUnreadMessageOnly(!isShowUnreadMessOnly))
        }
      />{' '}
      Show Unread Message Only
    </>
  );

  return (
    <div className='filter-control-checkbox'>
      {renderShowUnreadMessOnlyCheckBox()}
    </div>
  );
};

export default CheckBoxFilter;
