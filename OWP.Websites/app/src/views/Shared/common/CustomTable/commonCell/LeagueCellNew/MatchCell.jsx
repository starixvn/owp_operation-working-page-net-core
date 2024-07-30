import { Cell } from 'fixed-data-table-2';
import MatchContent from '../commonContentNew/MatchContent';
import { formatTime } from '../../../../DateTime/dateTimeHelper';
import { isUndefined } from 'lodash';
import PropTypes from 'prop-types';

const propTypes = {
  homeName: PropTypes.string,
  awayName: PropTypes.string,
  koTime: PropTypes.string,
  leagueName: PropTypes.string,
  isChild: PropTypes.bool,
  isRead: PropTypes.bool,
  children: PropTypes.node
};

const MatchCell = ({
  homeName = '',
  awayName = '',
  koTime = '',
  leagueName = '',
  isChild = false,
  isRead = false,
  children = null
}) => {
  const kickOffTime = formatTime(koTime, 'minute', 'h:mm A');
  const evenDate = formatTime(koTime, 'minute', 'MMM DD, YYYY');
  let matchClass = '';
  if (!isUndefined(isChild) && isChild) {
    matchClass = ' child-match';
  }
  return (
    <>
      <Cell
        className={
          !isRead
            ? `match-info-cell${matchClass} has-unread conversation-cell`
            : `match-info-cell${matchClass} cell conversation-cell`
        }>
        <MatchContent
          evenDate={evenDate}
          koTime={kickOffTime}
          matchName={{ homeName, awayName }}
          leagueName={leagueName}
        />
        {children}
      </Cell>
    </>
  );
};
MatchCell.propTypes = propTypes;
export default MatchCell;
