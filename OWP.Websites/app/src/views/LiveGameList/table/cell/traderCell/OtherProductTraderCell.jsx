import MaxbetInfo from '../../../component/MaxbetInfo';
import { useSelector } from 'react-redux';

const OtherProductTraderCell = ({ name, maxBet, traders, others, rowSpan }) => {
  const { userNameFilter } = useSelector(state => state.events);
  return (
    <>
      <MaxbetInfo
        name={name}
        maxbet={maxBet}
        traders={traders}
        others={others}
        rowSpan={rowSpan}
        userNameFilter={userNameFilter}
      />
    </>
  );
};

export default OtherProductTraderCell;
