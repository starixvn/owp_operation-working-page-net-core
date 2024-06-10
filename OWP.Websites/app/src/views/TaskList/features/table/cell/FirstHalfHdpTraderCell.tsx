import MaxbetInfo from '../../component/MaxbetInfo';
import { CellProps } from '../../../type/TTaskListTable';

const FirstHalfHdpTraderCell = ({ dataSource, rowSpan }: CellProps) => {
  const { MatchSetting, Assignment } = dataSource;
  const { Handicap1st } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name={null}
        maxbet={Handicap1st}
        traders={Assignment.Handicap1st}
        others={Assignment.Handicap1stOthers}
        rowSpan={rowSpan}
      />
    </>
  );
};

export default FirstHalfHdpTraderCell;
