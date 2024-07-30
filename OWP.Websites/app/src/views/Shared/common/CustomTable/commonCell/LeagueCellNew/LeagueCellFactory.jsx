import PropTypes from 'prop-types';
import OtherIssueCell from './OtherIssueCell';
import LeagueCell from './LeagueCell';
import MatchCell from './MatchCell';
import LogWorkCell from './LogWorkCell';
import IconOpenNewTab from '../../../../IconCustom/IconOpenNewTab';

const propTypes = {
  data: PropTypes.object.isRequired
};

const LeagueCellFactory = ({ data }) => {
  const {
    homeName,
    owpLeagueName,
    awayName,
    koTime,
    subject,
    isRead,
    eventType,
    ticketId
  } = data;

  switch (eventType) {
    case 1:
      return (
        <LeagueCell leagueName={owpLeagueName} isRead={isRead}>
          <IconOpenNewTab
            eventType={eventType}
            ticketId={ticketId}></IconOpenNewTab>
        </LeagueCell>
      );
    case 2:
    case 5:
      return (
        <MatchCell
          homeName={homeName}
          leagueName={owpLeagueName}
          awayName={awayName}
          koTime={koTime}
          isRead={isRead}>
          <IconOpenNewTab
            eventType={eventType}
            ticketId={ticketId}></IconOpenNewTab>
        </MatchCell>
      );
    case 6:
      return (
        <LogWorkCell data={data}>
          <IconOpenNewTab
            eventType={eventType}
            ticketId={ticketId}></IconOpenNewTab>
        </LogWorkCell>
      );
    default:
      return (
        <OtherIssueCell subject={subject || ''} isRead={isRead}>
          <IconOpenNewTab
            eventType={eventType}
            ticketId={ticketId}></IconOpenNewTab>
        </OtherIssueCell>
      );
  }
};

LeagueCellFactory.propTypes = propTypes;

export default LeagueCellFactory;
