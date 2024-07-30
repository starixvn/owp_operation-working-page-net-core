import OtherIssueCell from './OtherIssueCell';
import LeagueCell from './LeagueCell';
import MatchCell from './MatchCell';
import LogWorkCell from './LogWorkCell';
import { TTicket } from '../../../type';

const cellFactory = (props: TTicket) => {
  const {
    homeName,
    owpLeagueName,
    awayName,
    koTime,
    subject,
    isRead,
    eventType
  } = props;

  switch (eventType) {
    case 1:
      return <LeagueCell owpLeagueName={owpLeagueName} isRead={isRead} />;
    case 2:
    case 5:
      return (
        <MatchCell
          homeName={homeName}
          owpLeagueName={owpLeagueName}
          awayName={awayName}
          koTime={koTime}
          isRead={isRead}
          isChild={false}
        />
      );
    case 6:
      return <LogWorkCell {...props}/>;
    default:
      return <OtherIssueCell subject={subject || ''} isRead={isRead} />;
  }
};

export default cellFactory;
