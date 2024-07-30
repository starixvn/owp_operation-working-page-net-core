import MaxbetInfo from '../../../component/MaxbetInfo';
import { useSelector } from 'react-redux';

const FtOuTraderCell = ({ dataSource, rowSpan, rowIndex }) => {
  const { userNameFilter } = useSelector(state => state.events);
  const { MatchSetting, Assignment } = dataSource[rowIndex];
  const { OverUnder } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name='OU'
        maxbet={OverUnder}
        traders={Assignment.OverUnder}
        others={Assignment.OverUnderOthers}
        rowSpan={rowSpan}
        userNameFilter={userNameFilter}
      />
    </>
  );
};

export default FtOuTraderCell;
