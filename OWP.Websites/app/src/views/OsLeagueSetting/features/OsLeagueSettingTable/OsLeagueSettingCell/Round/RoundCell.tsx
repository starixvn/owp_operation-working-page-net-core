import { UncontrolledTooltip } from 'reactstrap';
import { CellProps } from '../../../../typing/tableType';

const RoundCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { roundName, leagueSettingId } = dataSource[rowIndex];
  return (
    <div className='round-cell'>
      <span id={`round-${leagueSettingId}`}>{roundName}</span>
      <UncontrolledTooltip
        placement='right'
        target={`round-${leagueSettingId}`}>
        {roundName}
      </UncontrolledTooltip>
    </div>
  );
};

export default RoundCell;
