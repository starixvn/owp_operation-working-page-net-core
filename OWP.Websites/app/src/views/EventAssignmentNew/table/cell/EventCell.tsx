import { TCellModel } from '../../type/type';
import { PlusIcon, ArrowRightIcon } from 'mdi-react';
import { Button } from 'reactstrap';
import store from '../../app/store';
import MatchInfo from '../../components/MatchInfo';
import EventPopup from '../../../Shared/EventPopup';
import { useAppDispatch } from '../../app/hooks';
import { setNewEventInfo } from '../../newEvent/newEventSlice';
import { setDetailEventInfo } from '../../eventDetail/eventDetailSlice';

const EventCell = (props: TCellModel) => {
  const { laTradingPage, tradingPage, servers } = store.getState()
    .tradingPageInfo.info as {
    laTradingPage: string;
    tradingPage: string;
    servers: {
      id: number;
      name: string;
      url: string;
      laPort: number;
      tradingPort: number | null;
    }[];
  };
  const dispatch = useAppDispatch();
  const {
    policy: {
      entities: { FullBetTypes, detailInformationPolicy }
    },
    allowAddEvent: { isAllowAddEvent, isYesterdayOrAfter }
  } = store.getState();
  const { rowIndex, dataSource } = props;
  const event = dataSource[rowIndex];

  const handleCopy = () => {
    const kickOffTime = `K.O Time: ${event.KickOffTime}`;
    const league = `League: ${event.League}`;
    const match = `Match: ${event.Home} -vs- ${event.Away}`;
    const matchNo = `Match ID: ${event.AdminSiteId ?? ''}`;
    const iptv = `TV/IPTV: ${event.IPTVCopy ?? ''} `;
    const provider = `Provider: ${event.Provider ?? ''}`;

    window.executeCopy(kickOffTime, league, match, matchNo, iptv, provider);
  };

  const allowToAddMinorMatch =
    isAllowAddEvent &&
    FullBetTypes.HasWrite &&
    event.OWPMatchId === event.ParentMatchId &&
    isYesterdayOrAfter;

  const dataLength = dataSource.length;
  const threeLastRow = [dataLength - 1, dataLength - 2, dataLength - 3];
  const className = threeLastRow.some(i => i === rowIndex)
    ? 'last-popup'
    : null;
  const serverSetting = servers.find(server => server.id === 0);

  return (
    <div className='table-cell' key={`Event${rowIndex}`}>
      {event.LeagueGroup && (
        <h4 className='league-group league-badge'>{event.LeagueGroup}</h4>
      )}
      <h4 className='league-group league-name'>{event.League}</h4>

      {allowToAddMinorMatch && (
        <a
          onClick={e => {
            e.preventDefault();
            dispatch(setNewEventInfo(event));
          }}
          rel='noopener noreferrer'
          className='add-specialEvent'>
          <PlusIcon size={13} />
        </a>
      )}
      {detailInformationPolicy.HasRead && event.IsAllowedToViewDetail && (
        <a
          onClick={e => {
            e.preventDefault();
            dispatch(setDetailEventInfo(event.OWPMatchId));
          }}
          className='link-livedetail'>
          <ArrowRightIcon size={13} />
        </a>
      )}

      <MatchInfo dataSource={dataSource} rowIndex={rowIndex} />

      {event.AdminSiteId && (
        <>
          <Button className='button-link-trader' id={`Event${rowIndex}`}>
            <span className='icon-link'></span>
          </Button>
          <EventPopup
            className={className}
            adminSiteId={event.AdminSiteId}
            isFantasy={event.IsFantasy}
            rowIndex={rowIndex}
            minorBettypeId={event.MinorBettypeId}
            league={event.League}
            laTradingPage={laTradingPage}
            tradingPage={tradingPage}
            serverSetting={serverSetting}
          />
        </>
      )}
      <Button className='btn-copy-content' onClick={handleCopy}>
        <span className='icon-content_copy'></span>
      </Button>
    </div>
  );
};

export default EventCell;
