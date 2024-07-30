import MaxbetInfo from '../../../component/MaxbetInfo';
import { useSelector } from 'react-redux';

const FtHdpTraderCell = ({ dataSource, rowSpan, rowIndex }) => {
  const { userNameFilter } = useSelector(state => state.events);
  const { MatchSetting, Assignment } = dataSource[rowIndex];
  const { Handicap } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name='HDP'
        maxbet={Handicap}
        traders={Assignment.Handicap}
        others={Assignment.HandicapOthers}
        rowSpan={rowSpan}
        userNameFilter={userNameFilter}
      />
    </>
  );
};

export default FtHdpTraderCell;
