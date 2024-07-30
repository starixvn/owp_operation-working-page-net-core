import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { UncontrolledTooltip } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';
import { Link } from 'react-router-dom';
import { buildLeagueDetailLink } from '../../../helper/commonHelper';
import LazyTeamInfo from '../teamInfo/TeamInfoWrapper';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LeagueCell = ({ dataSource, rowIndex }) => {
  const {
    LeagueName,
    IsBlackList,
    OWPLeagueId,
    IsActive,
    HasLinkLeagueAndTeam
  } = dataSource[rowIndex];
  return (
    <Cell key={uuidv4()} className='league-group league-name'>
        <Link
          to={{
            pathname: buildLeagueDetailLink(OWPLeagueId, LeagueName, IsActive)
          }}>
          {LeagueName}
        </Link>
      {IsBlackList && (
        <>
          <span id={`League${rowIndex}`} className='icon-alert' />
          <UncontrolledTooltip placement='top' target={`League${rowIndex}`}>
            black list
          </UncontrolledTooltip>
        </>
      )}
      {HasLinkLeagueAndTeam && (
        <LazyTeamInfo leagueName={LeagueName} owpLeagueId={OWPLeagueId} />
      )}
    </Cell>
  );
};

LeagueCell.propTypes = propTypes;
export default LeagueCell;
