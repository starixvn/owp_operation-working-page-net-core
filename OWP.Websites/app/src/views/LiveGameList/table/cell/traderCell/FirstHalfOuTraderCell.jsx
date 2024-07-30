import MaxbetInfo from '../../../component/MaxbetInfo';
import { useSelector } from 'react-redux';

const FirstHalfOuTraderCell = ({ dataSource, rowSpan, rowIndex }) => {
  const { userNameFilter } = useSelector(state => state.events);
  const { MatchSetting, Assignment } = dataSource[rowIndex];
  const { OverUnder1st } = MatchSetting;

  return (
    <>
      <MaxbetInfo
        name={null}
        maxbet={OverUnder1st}
        traders={Assignment.OverUnder1st}
        others={Assignment.OverUnder1stOthers}
        rowSpan={rowSpan}
        userNameFilter={userNameFilter}
      />
    </>
  );
};

export default FirstHalfOuTraderCell;
