import PropTypes from 'prop-types';

const propTypes = {
  evenDate: PropTypes.string,
  koTime: PropTypes.string,
  leagueName: PropTypes.string,
  matchName: PropTypes.object
};

const MatchContent = props => {
  const { evenDate, koTime, matchName, leagueName } = props;
  return (
    <span className='ticket-title-link'>
      {leagueName ? (
        <>
          <span>{leagueName} </span>
          <br />
          <span className='match-info-match-name'>
            {matchName.homeName} -vs- {matchName.awayName}
          </span>
          <br />
          <span>
            Event Date:
            <span className='match-info-date'>{evenDate}</span>
            <span className='match-info-KO'>K.O: {koTime}</span>
          </span>
        </>
      ) : (
        <span className='event-deleted'> This event has been deleted </span>
      )}
    </span>
  );
};
MatchContent.propTypes = propTypes;

export default MatchContent;
