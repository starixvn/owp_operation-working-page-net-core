import PropTypes from 'prop-types';
import MatchCell from './MatchCell';
import LeagueCell from './LeagueCell';
const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LeagueCellFactory = props => {
  const { dataSource, rowIndex } = props;
  const { InfoSiteMatchId } = dataSource[rowIndex];
  return InfoSiteMatchId ? <MatchCell {...props} /> : <LeagueCell {...props} />;
};

LeagueCellFactory.propTypes = propTypes;
export default LeagueCellFactory;
