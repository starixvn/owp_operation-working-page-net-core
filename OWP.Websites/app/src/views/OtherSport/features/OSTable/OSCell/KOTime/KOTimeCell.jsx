import PropTypes from 'prop-types';
import { useRef } from 'react';
import { UncontrolledTooltip } from 'reactstrap';
import { formatTime } from '../../../../../Shared/DateTime/dateTimeHelper';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const KOTimeCell = props => {
  const { dataSource, rowIndex } = props;
  const { dateTime, matchStatus } = dataSource[rowIndex];
  const statusRef = useRef();

  if (!dateTime) {
    return null;
  }
  const dateFormatted = formatTime(
    dateTime,
    'minute',
    'MMM DD'
  );
  const timeFormatted = formatTime(
    dateTime,
    'minute',
    'hh:mm A'
  );
  return (
    <div className='ko-time-cell'>
      <div className='time-info'>
        <div>{dateFormatted}</div>
        <div>{timeFormatted}</div>
      </div>
      <div className='match-status'>
        <span
          id={`match-status-${rowIndex}`}
          className={`icon-match-status icon-${matchStatus.toLowerCase()}`}
          ref={statusRef}></span>
        <UncontrolledTooltip placement='right' target={statusRef}>
          {matchStatus}
        </UncontrolledTooltip>
      </div>
    </div>
  );
};

KOTimeCell.propTypes = propTypes;
export default KOTimeCell;
