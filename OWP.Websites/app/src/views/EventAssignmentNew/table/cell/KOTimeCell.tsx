import { TCellModel } from '../../type/type';
import TimeOfMatch from '../../../Shared/LiveGameShared/TimeOfMatch';

const KOTimeCell = (props: TCellModel) => {
  const { rowIndex, dataSource } = props;
  const {
    OtherTime,
    OtherTimeStr,
    KickOffTime,
    KickOffTimeStr,
    InfoSiteTime,
    InfoSiteTimeDisplay
  } = dataSource[rowIndex];
  const propsTime = {
    KickOffTimeDisplay: KickOffTime,
    KickOffTime: KickOffTimeStr,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    OtherTimeDisplay: OtherTime,
    OtherTime: OtherTimeStr,
    rowIndex
  };
  return (
    <div className='kickoff-cell'>
      <span className='time-cell'>
        <TimeOfMatch {...propsTime} />
      </span>
    </div>
  );
};

export default KOTimeCell;
