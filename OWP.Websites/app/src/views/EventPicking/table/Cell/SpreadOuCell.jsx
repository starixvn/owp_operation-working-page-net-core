import PropTypes from 'prop-types';
import SpreadCell from './SpreadCell';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const SpreadOuCell = props => {
  const { rowIndex, dataSource } = props;
  const { spreadOu, infoSiteMatchId, leagueName,minorBettypeId, isChildMatch } = dataSource[rowIndex];
    dataSource[rowIndex];

  return (
    <SpreadCell
      spreadKey='spreadOu'
      infoSiteMatchId={infoSiteMatchId}
      spread={spreadOu}
      leagueName={leagueName}
      isChildMatch={isChildMatch}
      minorBettypeId={minorBettypeId}
    />
  );
};

SpreadOuCell.propTypes = propTypes;
export default SpreadOuCell;
