import MaxbetInfo from '../../component/MaxbetInfo';
import { CellProps } from '../../../type/TTaskListTable';

const FtHdpTraderCell = (props: CellProps) => {
  const { dataSource, rowSpan } = props;
  const { MatchSetting, Assignment } = dataSource;
  const { Handicap } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name='HDP'
        maxbet={Handicap}
        traders={Assignment.Handicap}
        others={Assignment.HandicapOthers}
        rowSpan={rowSpan}
      />
    </>
  );
};

export default FtHdpTraderCell;
