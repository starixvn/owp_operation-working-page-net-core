import OtherIssueCell from './OtherIssueCell';
import MatchCell from './MatchCell';
import { TTicket } from '../../../type';


const LogWorkCell = (props: TTicket) => {
  const { isChild, homeName, leagueName, awayName, koTime, subject, isRead } = props;

  return leagueName ? (
    <MatchCell
      homeName={homeName}
      owpLeagueName={leagueName}
      awayName={awayName}
      koTime={koTime}
      isRead={isRead}
      isChild={isChild}
    />
  ) : (
    <OtherIssueCell subject={subject || ''} isRead={isRead} />
  );
};


export default LogWorkCell;
