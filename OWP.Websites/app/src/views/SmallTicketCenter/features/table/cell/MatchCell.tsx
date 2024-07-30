import MatchContent from '../../../../Shared/common/CustomTable/commonCell/commonContentNew/MatchContent';
import { isUndefined } from 'lodash';
import { formatTime } from '../../../../Shared/DateTime/dateTimeHelper';
import { TTicket } from '../../../type';

const MatchCell = ({
  homeName = '',
  awayName = '',
  koTime = '',
  owpLeagueName = '',
  isChild = false,
  isRead = false
}: Omit<TTicket, 'ticketId'|'subject'|'leagueName'|'eventType'>) => {
  const kickOffTime = formatTime(koTime, 'minute', 'h:mm A');
  const evenDate = formatTime(koTime, 'minute', 'MMM DD, YYYY');
  let matchClass = '';
  if (!isUndefined(isChild) && isChild) {
    matchClass = ' child-match';
  }
  return (
    <span
      className={
        !isRead
          ? `match-info-cell${matchClass} has-unread conversation-cell`
          : `match-info-cell${matchClass} cell conversation-cell`
      }>
      <MatchContent
        evenDate={evenDate}
        koTime={kickOffTime}
        matchName={{ homeName, awayName }}
        leagueName={owpLeagueName}
      />
    </span>
  );
};
export default MatchCell;
