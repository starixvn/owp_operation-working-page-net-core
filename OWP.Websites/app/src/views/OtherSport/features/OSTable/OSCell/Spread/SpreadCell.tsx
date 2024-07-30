import { DefaultCellTrader, DefaultCellProvider } from '../../TableSchema.json';
import { DataSource, MatchSetting } from '../Share/Type';

type Props = {
  dataSource: Required<Array<DataSource>>,
  rowIndex: Required<number>
};

const SpreadCell = (props: Props): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { traderMaxbetModels } = dataSource[rowIndex];
  const traderMaxbetModelsCanDisplay = traderMaxbetModels.filter(traderMaxbet => !traderMaxbet.hiden || typeof traderMaxbet.hiden === 'undefined');
  const maxBetList = traderMaxbetModelsCanDisplay.map(e => e.matchSettings);

  const displaySpread = (matchSettings: Array<MatchSetting>) => {
    return matchSettings.map((matchSetting, index) => {
      const { betTypeName, spread } = matchSetting;
      return (
        <li
          key={index}>
          {betTypeName
            ? (spread ? `${betTypeName}: ${spread}` : null)
            : spread}
        </li>
      );
    });
  };

  const renderLisSpreadElement = (matchSettings: Array<Array<MatchSetting>>, length: number) => {
    const height = length > 1 ? DefaultCellTrader.height : DefaultCellProvider.height;
    return matchSettings.map((matchSettingsByBettype, index) => {
      return (
        <li
          style={{
            height
          }}
          className='spread'
          key={index}>
          <ul className='minor-spread'>
            {displaySpread(matchSettingsByBettype)}
          </ul>
        </li>
      );
    });
  };

  return (
    <ul className='list-spreads-os'>
      {renderLisSpreadElement(maxBetList, maxBetList.length)}
    </ul>
  );
};

export default SpreadCell;
