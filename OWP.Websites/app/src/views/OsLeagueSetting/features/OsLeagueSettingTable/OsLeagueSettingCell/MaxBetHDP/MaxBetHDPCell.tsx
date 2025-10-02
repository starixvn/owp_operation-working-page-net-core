import { numberWithCommas } from '../../../../helper/OsLeagueSettingHelper';
import { CellProps } from '../../../../typing/tableType';
import StatusIcon from '../Status/StatusIcon';

const MaxBetHDPCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { maxBetHDP, isActiveMaxBet } = dataSource[rowIndex];
  return (
    <div className='maxbet-hdp-cell'>
      <span className='maxbet-status'>
        <StatusIcon isActive={isActiveMaxBet} />
      </span>
      <span className='maxbet-value'>
        {maxBetHDP ? numberWithCommas(maxBetHDP) : ''}
      </span>
    </div>
  );
};

export default MaxBetHDPCell;
