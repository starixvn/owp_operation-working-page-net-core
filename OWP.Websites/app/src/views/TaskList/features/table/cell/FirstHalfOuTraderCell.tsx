import MaxbetInfo from '../../component/MaxbetInfo';
import { CellProps } from '../../../type/TTaskListTable';

const FirstHalfOuTraderCell = ({ dataSource, rowSpan }: CellProps) => {
  const { MatchSetting, Assignment } = dataSource;
  const { OverUnder1st } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name={null}
        maxbet={OverUnder1st}
        traders={Assignment.OverUnder1st}
        others={Assignment.OverUnder1stOthers}
        rowSpan={rowSpan}
      />
    </>
  );
};

export default FirstHalfOuTraderCell;
