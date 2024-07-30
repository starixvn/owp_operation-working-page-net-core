import PropTypes from 'prop-types';
import { markAllTicketsAsRead } from '../../../services/service';
import {
  updatedIsReadAllRows,
  useStableDispatch
} from '../../../ImportTicketCenterHelperNative';
import ToastsWrapper from '../../../../components/ToastrWrapper';
const propTypes = {
  actorId: PropTypes.number.isRequired
};

const MarkAllReadButton = ({ actorId }) => {
  const dispatch = useStableDispatch();

  const handleOnClick = async () => {
    const confirm = await ToastsWrapper.confirm(
      'Are you sure you want to mark all new messages as read?'
    );
    if (confirm?.dismiss) {
      return;
    }
    const { status } = await markAllTicketsAsRead(actorId);
    status === 200 && dispatch(updatedIsReadAllRows());
  };

  return (
    <div>
      <button className='mark-all-tickets' onClick={handleOnClick}>
        Mark all as read
      </button>
    </div>
  );
};

MarkAllReadButton.propTypes = propTypes;

export default MarkAllReadButton;
