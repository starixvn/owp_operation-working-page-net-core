import { useRef } from 'react';
import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';
import AlertIcon from 'mdi-react/AlertIcon';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const handleNavigateToLeagueSetting = leagueName => {
  const url = `LeagueManagement?leagueName=${leagueName}&isEqualLeague=true`;
  window.open(url, '_blank');
};

const WarningIcon = ({ leagueName, title, iconRef }) => (
  <>
    <span
      ref={iconRef}
      className='warning-icon'
      onClick={() => handleNavigateToLeagueSetting(leagueName)}>
      <AlertIcon size={16} color='#ff9800' />
    </span>
    <UncontrolledTooltip placement='top' target={iconRef}>
      {title}
    </UncontrolledTooltip>
  </>
);

WarningIcon.propTypes = {
  leagueName: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  iconRef: PropTypes.object.isRequired
};

const MatchCell = ({ rowIndex, dataSource }) => {
  const {
    isClubFriendlyLeague,
    homeName,
    awayName,
    homeMainLeague,
    awayMainLeague,
    homeMainLeagueSetting,
    awayMainLeagueSetting
  } = dataSource[rowIndex];
  const refHomeNoOfferLive = useRef(null);
  const refAwayNoOfferLive = useRef(null);
  const refHomeBlackList = useRef(null);
  const refAwayBlackList = useRef(null);

  const renderTeamWarnings = (
    leagueName,
    mainLeagueSetting,
    refNoOfferLive,
    refBlackList
  ) => {
    if (!mainLeagueSetting || !isClubFriendlyLeague) {
      return null;
    }
    return (
      <>
        {mainLeagueSetting.isNoOfferLive && (
          <WarningIcon
            leagueName={leagueName}
            title='No Offer Live'
            iconRef={refNoOfferLive}
          />
        )}
        {mainLeagueSetting.isBlackList && (
          <WarningIcon
            leagueName={leagueName}
            title='Black List'
            iconRef={refBlackList}
          />
        )}
      </>
    );
  };

  return (
    <div className='match-cell'>
      <div className='home'>
        {homeName}
        {renderTeamWarnings(
          homeMainLeague,
          homeMainLeagueSetting,
          refHomeNoOfferLive,
          refHomeBlackList
        )}
      </div>
      <div className='away'>
        {awayName}
        {renderTeamWarnings(
          awayMainLeague,
          awayMainLeagueSetting,
          refAwayNoOfferLive,
          refAwayBlackList
        )}
      </div>
    </div>
  );
};

MatchCell.propTypes = propTypes;
export default MatchCell;
