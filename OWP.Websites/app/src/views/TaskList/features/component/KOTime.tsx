import { Fragment } from 'react';
import { KOTimeCellProps } from '../../type/TTaskListTable';
import TimeOfMatch from '../../../Shared/LiveGameShared/TimeOfMatch';

const KOTime = (props: KOTimeCellProps) => {
  const { flagElement, eventDateElement, dataSource, rowIndex } = props;
  const {
    KickOffTimeDisplay,
    KickOffTime,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    OtherTimeDisplay,
    OtherTime
  } = dataSource;

  const propsTime = {
    KickOffTimeDisplay,
    KickOffTime,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    OtherTimeDisplay,
    OtherTime,
    rowIndex
  };

  return (
    <>
      <div className='ko-time-cell'>
        {eventDateElement}
        <TimeOfMatch {...propsTime} />
        {flagElement}
      </div>
    </>
  );
};

export default KOTime;
