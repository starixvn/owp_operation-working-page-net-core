import { useContext } from 'react';
import { Button } from 'reactstrap';
import { CellProps } from '../../../type/TTaskListTable';
import EventPopup from '../../../../Shared/EventPopup';
import { AppContext } from '../../../../Shared/Context';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { openPopupDetail } from '../taskListSlice';
import { two, three } from '../../const/TableSchema.json';
import store from '../../../app/storeNative';
import { MessageTextIcon } from 'mdi-react';
import { UncontrolledTooltip } from 'reactstrap';

const MatchContainLinkDetailCell = (props: CellProps) => {
  const { rowIndex } = props;
  const {
    Home,
    Away,
    MatchCode,
    AdminSiteId,
    KickOffTimeDisplay,
    League,
    IPTVCopy,
    Provider,
    IsFantasy,
    MinorBettypeId,
    IsAllowedToViewDetail,
    OWPMatchId
  } = props.dataSource;

  const SOP = (props.dataSource as any).SOP;
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
  const serverSetingId = store.getState().tradingPageInfo.serverSetingId;

  const { detailInformationPolicy } = useContext(AppContext);
  const dispatch = useStableDispatch();
  const dataLength = props.dataLength ?? 0;
  const threeLastRow = [dataLength - 1, dataLength - two, dataLength - three];

  const className = threeLastRow.some(i => i === rowIndex)
    ? 'last-popup'
    : null;

  const renderLeagueHomeAway = () => {
    return (
      <div
        onClick={handleOpenLiveDetail}
        className='match-link js-openModalLiveDetail'>
        <p className='home-team'>{Home}</p>
        <p className='away-team'>{Away}</p>
      </div>
    );
  };

  const handleCopy = () => {
    const koTime = `K.O Time: ${KickOffTimeDisplay}`;
    const leagueCopy = `League: ${League}`;
    const match = `Match: ${Home} -vs- ${Away}`;
    const matchId = `Match ID: ${AdminSiteId ?? ''}`;
    const iptv = `TV/IPTV: ${IPTVCopy ?? ''} `;
    const providerCopy = `Provider: ${Provider ?? ''}`;

    window.executeCopy(koTime, leagueCopy, match, matchId, iptv, providerCopy);
  };

  const handleOpenLiveDetail = () => {
    detailInformationPolicy.HasRead &&
      IsAllowedToViewDetail &&
      dispatch(openPopupDetail(OWPMatchId));
  };

  const serverSetting = servers.find(server => server.id === serverSetingId);

  const renderLinkIcon = () => {
    return (
      <>
        <Button className='button-link-trader' id={`Event${rowIndex}`}>
          <span className='icon-link'></span>
        </Button>
        <EventPopup
          className={className}
          adminSiteId={AdminSiteId}
          isFantasy={IsFantasy}
          rowIndex={rowIndex}
          minorBettypeId={MinorBettypeId}
          league={League}
          laTradingPage={laTradingPage}
          tradingPage={tradingPage}
          serverSetting={serverSetting}
        />
      </>
    );
  };

  const renderIcon = () => {
    return (
      <>
        {SOP && (
          <>
            <span id={`taskList-sop-${rowIndex}`}>
              <MessageTextIcon className='icon-sop' />
            </span>
            <UncontrolledTooltip target={`taskList-sop-${rowIndex}`}>
              {SOP}
            </UncontrolledTooltip>
          </>
        )}
        <Button className='btn-copy-content' onClick={handleCopy}>
          <span className='icon-content_copy'></span>
        </Button>
      </>
    );
  };

  return (
    <>
      {renderLeagueHomeAway()}
      <p className='match-code'>{MatchCode}</p>
      <div className='list-button'>
        {Boolean(AdminSiteId) && renderLinkIcon()}
        {renderIcon()}
      </div>
    </>
  );
};

export default MatchContainLinkDetailCell;
