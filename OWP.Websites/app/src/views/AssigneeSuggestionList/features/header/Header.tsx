import { shallow } from 'zustand/shallow';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import useStore from '../../app/useStoreNative';
import SearchAssignee from './SearchAssignee';
import ColorExplain from './ColorExplain';
import Filter from './Filter';

const Header = () => {
  const {
    workScheduleInfo: { matchInfo }
  } = useStore(
    state => ({
      workScheduleInfo: state.workScheduleInfo
    }),
    shallow
  );
  const {
    matchNo,
    leagueName,
    homeName,
    awayName,
    koTime,
    maxbetHDPInString,
    bettypeName
  } = matchInfo;

  const searchParams = new URLSearchParams(document.location.search);
  const otherTime = searchParams.get('otherTime');
  const formattedDate = getLocaleDateTime(koTime, "MMM DD, YYYY");
  const formattedTime = getLocaleDateTime(koTime, "hh:mm A");

  return (
    <>
      <div className='match-info'>
        {leagueName && (
          <>
            <b>
              #{matchNo} {leagueName} | {homeName} -vs- {awayName}
            </b>
            &nbsp;
            <span className="time">
              {`Event Date: ${formattedDate}${otherTime ? ' - Other time:' : ''} ${formattedTime} `}
            </span>
            <span className="maxbet-string"> {maxbetHDPInString}</span>
            &nbsp;
            <span className='bet-type'>{bettypeName}</span>
          </>
        )}
      </div>
      <div className='filter__info'>
        <Filter />
        <SearchAssignee />
        <ColorExplain />
      </div>


    </>
  );
};

export default Header;
