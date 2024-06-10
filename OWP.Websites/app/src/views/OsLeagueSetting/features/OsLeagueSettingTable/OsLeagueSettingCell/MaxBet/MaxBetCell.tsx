import { numberWithCommas } from '../../../../helper/OsLeagueSettingHelper';
import { CellProps } from '../../../../typing/tableType';
import StatusIcon from '../Status/StatusIcon';

const MaxBetCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { maxBetHDP, maxBetOU, isActiveMaxBet } = dataSource[rowIndex];
  return (
    <div className='maxbet-cell'>
      <span className='maxbet-status'>
        <StatusIcon isActive={isActiveMaxBet} />
      </span>
      <span className='maxbet-wrapper'>
        <span className='hdp-maxbet'>
          {maxBetHDP && `HDP: ${numberWithCommas(maxBetHDP)}`}
        </span>
        <span className='ou-maxbet'>
          {maxBetOU && `OU:${numberWithCommas(maxBetOU)}`}
        </span>
      </span>
    </div>
  );
};

export default MaxBetCell;
