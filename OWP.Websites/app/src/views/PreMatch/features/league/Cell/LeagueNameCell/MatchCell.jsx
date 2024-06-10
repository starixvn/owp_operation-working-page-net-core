import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import { UncontrolledTooltip } from 'reactstrap';
import { formatTime } from '../../../../ImportPrematchHelperNative';
const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MatchCell = props => {
  const { dataSource, rowIndex } = props;
  const { HomeName, AwayName, EventDateTime, MatchStatus } = dataSource[rowIndex];
  const koTimeFormat = formatTime(
    EventDateTime,
    'minute',
    'hh:mm A'
  );

  const matchName = (
    <>
      <span className='KO-time'>{koTimeFormat} </span>
      <span>
        {HomeName} -vs- {AwayName}
      </span>
    </>
  );

  const customizeMatchStatus = () => {
    switch (MatchStatus) {
      case 'LIVE':
      case 'LIVE(!)':
        return MatchStatus;
      default:
        return <span className='prematch-nonelive'>
          {MatchStatus}
        </span>;
    }
  };

  const matchStatus = (
    <>
      <span className='prematch-match-status'>
        {customizeMatchStatus()}
      </span>
    </>
  );

  return (
    <Cell key={nanoid()}>
      <div>
        <label id={`name${rowIndex}`} className='header-LeagueName match-row'>
          {matchStatus}
          {matchName}
        </label>
        <UncontrolledTooltip placement='top' target={`name${rowIndex}`}>
          {matchName}
        </UncontrolledTooltip>
      </div>
    </Cell>
  );
};

MatchCell.propTypes = propTypes;
export default MatchCell;
