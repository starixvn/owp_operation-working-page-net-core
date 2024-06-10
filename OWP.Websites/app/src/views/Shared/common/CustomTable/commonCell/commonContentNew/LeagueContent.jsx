import PropTypes from 'prop-types';

const propTypes = {
  leagueName: PropTypes.string.isRequired
};

const LeagueContent = props => {
  const { leagueName } = props;
  return <span className='league-Name ticket-title-link'>{leagueName}</span>;
};
LeagueContent.propTypes = propTypes;

export default LeagueContent;
