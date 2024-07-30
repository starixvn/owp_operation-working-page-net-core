import MaxbetInfo from '../../component/MaxbetInfo';
import { CellProps } from '../../../type/TTaskListTable';

const FtHdpTraderCell = (props: CellProps) => {
  const { dataSource, rowSpan } = props;
  const { MatchSetting, Assignment } = dataSource;
  const { OverUnder } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name='OU'
        maxbet={OverUnder}
        traders={Assignment.OverUnder}
        others={Assignment.OverUnderOthers}
        rowSpan={rowSpan}
      />
    </>
  );
};

export default FtHdpTraderCell;
