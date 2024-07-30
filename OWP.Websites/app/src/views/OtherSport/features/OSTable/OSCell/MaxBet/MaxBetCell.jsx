import PropTypes from 'prop-types';
import { isEqual } from 'lodash';
import { UncontrolledTooltip } from 'reactstrap';
import Maxbet from './Maxbet.json';
import { DefaultCellTrader, DefaultCellProvider } from '../../TableSchema.json';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MaxBetCell = props => {
  const { dataSource, rowIndex } = props;
  const { traderMaxbetModels, sport } = dataSource[rowIndex];
  const traderMaxbetModelsCanDisplay = traderMaxbetModels.filter(
    traderMaxbet =>
      !traderMaxbet.hiden || typeof traderMaxbet.hiden === 'undefined'
  );
  const maxBetList = traderMaxbetModelsCanDisplay.map(e => e.matchSettings);

  const displayMaxbetSpecific = ({ maxbet }) => {
    const { display, tooltip } = maxbet;
    return (
      <>
        <span className='maxbet-specific' id={`maxbet-${rowIndex}`}>
          <i>{display}</i>
        </span>
        <UncontrolledTooltip placement='right' target={`maxbet-${rowIndex}`}>
          {tooltip}
        </UncontrolledTooltip>
      </>
    );
  };

  const displayMaxbetNormal = matchSettings => {
    return matchSettings.map((matchSetting, index) => {
      const { betTypeName, maxBet } = matchSetting;
      const maxbetDisplay = maxBet ? `${maxBet.toLocaleString()}` : null;

      return (
        <li key={index}>
          {betTypeName ? `${betTypeName}: ${maxbetDisplay}` : maxbetDisplay}
        </li>
      );
    });
  };

  const displayMaxbet = source => {
    for (const key in Maxbet) {
      if (isEqual(key, sport)) {
        return displayMaxbetSpecific(Maxbet[key]);
      }
    }
    return <ul className='minor-maxbet'>{displayMaxbetNormal(source)}</ul>;
  };

  const renderListMaxbetElement = (matchSettings, length) => {
    const height =
      length > 1 ? DefaultCellTrader.height : DefaultCellProvider.height;
    return matchSettings.map((matchSettingsByBettype, index) => {
      return (
        <li
          style={{
            height
          }}
          className='maxbet'
          key={index}>
          {displayMaxbet(matchSettingsByBettype)}
        </li>
      );
    });
  };

  return (
    <ul className='list-maxbets-os'>
      {renderListMaxbetElement(maxBetList, maxBetList.length)}
    </ul>
  );
};

MaxBetCell.propTypes = propTypes;

export default MaxBetCell;
