import OtherIssueCell from './OtherIssueCell';
import MatchCell from './MatchCell';
import PropTypes from 'prop-types';

const propTypes = {
  data: PropTypes.object.isRequired,
  children: PropTypes.node
};

const LogWorkCell = ({ data, children = null }) => {
  const { homeName, leagueName, awayName, koTime, subject, isRead } = data;

  return (
    <>
      {leagueName ? (
        <MatchCell
          homeName={homeName}
          leagueName={leagueName}
          awayName={awayName}
          koTime={koTime}
          isRead={isRead}>
          {children}
        </MatchCell>
      ) : (
        <OtherIssueCell subject={subject || ''} isRead={isRead}>
          {children}
        </OtherIssueCell>
      )}
    </>
  );
};

LogWorkCell.propTypes = propTypes;
export default LogWorkCell;
