import { isEqual } from 'lodash';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Select from 'react-select';
import { updateTicketStatus } from './chatBoardThunkNative';
import { STATUS } from './const/index.json';
import ToastsWrapper from '../../components/ToastrWrapper';
import PropTypes from 'prop-types';

const TicketStatus = ({ policy = 'policy' }) => {
  const ticketDetail = useSelector(state => state.chatBoard.ticketDetail);
  const { ticketStatus, ticketId } = ticketDetail;
  const [currentStatus, setCurrentStatus] = useState();
  const { permission } = useSelector(state => state[policy]);
  const { HasWrite, ActorId } = permission;
  const dispatch = useDispatch();

  useEffect(() => {
    setCurrentStatus({ value: ticketStatus, label: ticketStatus });
  }, [ticketStatus]);

  const onSelectedChange = async event => {
    if (isEqual(ticketStatus, event.value)) {
      return;
    }

    const model = {
      ticketId,
      ticketStatus: event.value,
      actorId: ActorId,
      ticketInfo: ticketDetail
    };
    const actionResult = await dispatch(updateTicketStatus(model));

    if (updateTicketStatus.rejected.match(actionResult)) {
      ToastsWrapper.showError(actionResult.payload);
      return;
    }

    setCurrentStatus(event);
  };

  return (
    <>
      <label>
        Status:<span className='required'>*</span>
      </label>
      <br />
      <div className='select'>
        <Select
          options={STATUS}
          defaultValue={STATUS[0]}
          value={currentStatus}
          onChange={onSelectedChange}
          isDisabled={!HasWrite}
        />
      </div>
    </>
  );
};

TicketStatus.propTypes = {
  policy: PropTypes.string
};

export default TicketStatus;
