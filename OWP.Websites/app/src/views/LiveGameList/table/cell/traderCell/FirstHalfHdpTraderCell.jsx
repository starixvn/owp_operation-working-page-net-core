import MaxbetInfo from '../../../component/MaxbetInfo';
import { useSelector } from 'react-redux';

const FirstHalfHdpTraderCell = ({ dataSource, rowSpan, rowIndex }) => {
  const { userNameFilter } = useSelector(state => state.events);
  const { MatchSetting, Assignment } = dataSource[rowIndex];
  const { Handicap1st } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name={null}
        maxbet={Handicap1st}
        traders={Assignment.Handicap1st}
        others={Assignment.Handicap1stOthers}
        rowSpan={rowSpan}
        userNameFilter={userNameFilter}
      />
    </>
  );
};

export default FirstHalfHdpTraderCell;
