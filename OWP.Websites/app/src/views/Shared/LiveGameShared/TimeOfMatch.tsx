import { Fragment } from 'react';
import { isEqual } from 'lodash';

type typeProps = {
  KickOffTimeDisplay: string;
  KickOffTime: string;
  InfoSiteTimeDisplay: string | null;
  InfoSiteTime: string | null;
  OtherTimeDisplay: string | null;
  OtherTime: string | null;
  rowIndex: number;
};

const TimeOfMatch = (props: typeProps) => {
  const {
    KickOffTimeDisplay,
    KickOffTime,
    InfoSiteTimeDisplay,
    InfoSiteTime,
    OtherTimeDisplay,
    OtherTime,
    rowIndex
  } = props;

  const conditionToShowInfoSiteTime =
    !isEqual(KickOffTime, InfoSiteTime) && !isEqual(OtherTime, InfoSiteTime);
  const timeArray = [
    {
      time: KickOffTime,
      element: <p>{KickOffTimeDisplay}</p>
    },
    {
      time: OtherTime,
      element: OtherTime && (
        <p>
          <span className='badge-label other-time'>{OtherTimeDisplay}</span>
        </p>
      )
    },
    {
      time: InfoSiteTime,
      element: conditionToShowInfoSiteTime && (
        <p>
          <span className='badge-label info-site-time'>
            {InfoSiteTimeDisplay}
          </span>
        </p>
      )
    }
  ];
  timeArray.sort((a, b) =>
    (a.time === null || a.time === undefined) ? 1 : (b.time === null || b.time === undefined) ? -1 : a.time.localeCompare(b.time)
  );
  return (
    <>
      {timeArray.map(
        item =>
          item.time &&
          item.element && (
            <Fragment key={`${item.time}-${rowIndex}`}>{item.element}</Fragment>
          )
      )}
    </>
  );
};

export default TimeOfMatch;
