import PropTypes from 'prop-types';
import SpreadCell from './SpreadCell';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const SpreadOuCell = props => {
  const { rowIndex, dataSource } = props;
  const { spreadOu, infoSiteMatchId, leagueName, isChildMatch } =
    dataSource[rowIndex];

  return (
    <SpreadCell
      spreadKey='spreadOu'
      infoSiteMatchId={infoSiteMatchId}
      spread={spreadOu}
      leagueName={leagueName}
      isChildMatch={isChildMatch}
    />
  );
};

SpreadOuCell.propTypes = propTypes;
export default SpreadOuCell;
