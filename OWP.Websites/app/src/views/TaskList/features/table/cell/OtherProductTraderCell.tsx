import MaxbetInfo from '../../component/MaxbetInfo';
import { OtherProductProps } from '../../../type/TTaskListTable';

const OtherProductTraderCell = ({
  name,
  maxBet,
  traders,
  others,
  rowSpan
}: OtherProductProps) => {
  return (
    <>
      <MaxbetInfo
        name={name}
        maxbet={maxBet}
        traders={traders}
        others={others}
        rowSpan={rowSpan}
      />
    </>
  );
};

export default OtherProductTraderCell;
