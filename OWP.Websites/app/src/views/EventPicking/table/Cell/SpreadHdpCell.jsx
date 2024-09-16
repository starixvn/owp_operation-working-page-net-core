import PropTypes from 'prop-types';
import SpreadCell from './SpreadCell';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const SpreadHdpCell = props => {
  const { rowIndex, dataSource } = props;
  const {
    spreadHdp,
    infoSiteMatchId,
    leagueName,
    minorBettypeId,
    isChildMatch
  } = dataSource[rowIndex];

  return (
    <SpreadCell
      spreadKey='spreadHdp'
      infoSiteMatchId={infoSiteMatchId}
      spread={spreadHdp}
      leagueName={leagueName}
      isChildMatch={isChildMatch}
      minorBettypeId={minorBettypeId}
    />
  );
};

SpreadHdpCell.propTypes = propTypes;
export default SpreadHdpCell;
