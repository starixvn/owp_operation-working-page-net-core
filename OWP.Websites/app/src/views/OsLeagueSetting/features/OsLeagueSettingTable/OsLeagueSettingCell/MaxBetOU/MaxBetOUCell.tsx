import { numberWithCommas } from '../../../../helper/OsLeagueSettingHelper';
import { CellProps } from '../../../../typing/tableType';

const MaxBetOUCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { maxBetOU, isActiveMaxBet } = dataSource[rowIndex];
  return (
    <div className='maxbet-ou-cell'>
      <span className='maxbet-value'>
        {maxBetOU ? numberWithCommas(maxBetOU) : ''}
      </span>
    </div>
  );
};

export default MaxBetOUCell;
