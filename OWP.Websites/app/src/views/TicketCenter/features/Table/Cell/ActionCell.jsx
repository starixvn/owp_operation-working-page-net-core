import { useState } from 'react';
import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';
import StatusPopup from '../../StatusHistory/StatusPopup';
import {
  pinTicket,
  deleteTicket,
  useStableDispatch,
  refreshEventTickets
} from '../../../ImportTicketCenterHelperNative';
import store from '../../../app/storeNative';
import ToastsWrapper from '../../../../components/ToastrWrapper';
const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const ActionCell = props => {
  const { dataSource, rowIndex } = props;
  const { ticketId, isRead, isPinned } = dataSource[rowIndex];
  const {
    permission: { ActorId }
  } = store.getState().pagePolicy;
  const {
    permission: { HasWrite }
  } = store.getState().deletePolicy;

  const { filterModel } = store.getState().signalR;

  const [isOpenStatusHistory, setIsOpenStatusHistory] = useState(false);
  const dispatch = useStableDispatch();

  const handleOpenStatusHistory = event => {
    event.stopPropagation();
    setIsOpenStatusHistory(true);
  };

  const handleCloseStatusHistory = () => setIsOpenStatusHistory(false);

  const handlePinTicket = (event, isPinnedToTop) => {
    event.stopPropagation();
    const model = {
      actorId: ActorId,
      ticketId,
      isPinned: isPinnedToTop,
      rowIndex
    };
    dispatch(pinTicket(model));
    dispatch(refreshEventTickets(filterModel));
  };

  const handleDeleteTicket = async event => {
    event.stopPropagation();
    const confirm = await ToastsWrapper.confirm(
      `Are you sure that you want to delete this ticket ID:${ticketId}?`
    );
    if (confirm?.dismiss) {
      return;
    }
    const model = {
      actorId: ActorId,
      ticketId
    };
    await dispatch(deleteTicket(model));
    await dispatch(refreshEventTickets(filterModel));
  };

  return (
    <Cell className={!isRead ? 'has-unread' : 'cell'}>
      <div className='action-cell'>
        {isPinned ? (
          <span
            id={`pin-icon-${ticketId}`}
            className='icon-unpin'
            onClick={event => handlePinTicket(event, false)}
          />
        ) : (
          <span
            id={`unpin-icon-${ticketId}`}
            className='icon-pin'
            onClick={event => handlePinTicket(event, true)}
          />
        )}
        <span
          className='icon-history'
          id={`history-icon-${ticketId}`}
          size={16}
          onClick={handleOpenStatusHistory}
        />
        {HasWrite && (
          <span
            className='icon-delete'
            id={`delete-icon-${ticketId}`}
            size={16}
            onClick={handleDeleteTicket}
          />
        )}
        {isOpenStatusHistory && (
          <StatusPopup
            ticketId={ticketId}
            handleClosePopup={handleCloseStatusHistory}
          />
        )}
      </div>
    </Cell>
  );
};

ActionCell.propTypes = propTypes;
export default ActionCell;
