import { ArrowBackIcon } from 'mdi-react';
import { formatTime } from '../DateTime/dateTimeHelper';
import PropTypes from 'prop-types';

const TicketDetailHeader = ({
  ticketId = '',
  subject = '',
  homeName = '',
  awayName = '',
  eventName = '',
  koTime = '',
  isSmallBoard,
  backToSmallTicket
}) => {
  const backIcon = isSmallBoard ? (
    <span
      className='back__icon'
      style={{ marginRight: '5px', color: 'gray' }}
      onClick={backToSmallTicket}>
      <ArrowBackIcon fontSize='large'></ArrowBackIcon>
    </span>
  ) : undefined;

  if (subject) {
    return (
      <div>
        {backIcon}
        <h3 className='text-bold h3'>#{`${ticketId} - ${subject}`}</h3>
      </div>
    );
  }

  if (homeName) {
    const kickOffTime = formatTime(koTime, 'minute', 'h:mm A');
    const evenDate = formatTime(koTime, 'minute', 'MMM DD, YYYY');
    return (
      <>
        <div>
          {backIcon}
          <h3 className='text-bold'>#{`${ticketId} - ${eventName}`}</h3>
        </div>
        <div className='event-title'>
          <span className='event-name text-bold h3'>
            {' '}
            {homeName} -vs- {awayName}
          </span>
          <span className='event-date'>
            <span>{`(${evenDate}`}</span>
            <span>{`- ${kickOffTime})`}</span>
          </span>
        </div>
      </>
    );
  }

  if (eventName) {
    return (
      <div>
        {backIcon}
        <h3 className='text-bold'>#{`${ticketId} - ${eventName}`}</h3>
      </div>
    );
  }

  return (
    <div>
      {backIcon}
      <h3 className='text-bold'>
        #{`${ticketId} - This event has been deleted`}
      </h3>
    </div>
  );
};

TicketDetailHeader.propTypes = {
  ticketId: PropTypes.number.isRequired,
  subject: PropTypes.string,
  homeName: PropTypes.string,
  awayName: PropTypes.string,
  eventName: PropTypes.string,
  koTime: PropTypes.instanceOf(Date),
  isSmallBoard: PropTypes.bool,
  backToSmallTicket: PropTypes.func.isRequired
};

export default TicketDetailHeader;
