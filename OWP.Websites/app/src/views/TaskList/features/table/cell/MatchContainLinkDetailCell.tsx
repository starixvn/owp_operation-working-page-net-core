import { useContext } from 'react';
import { Button } from 'reactstrap';
import { CellProps } from '../../../type/TTaskListTable';
import EventPopup from '../../../../Shared/EventPopup';
import { AppContext } from '../../../../Shared/Context';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { openPopupDetail } from '../taskListSlice';
import { two, three } from '../../const/TableSchema.json';

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
        />
      </>
    );
  };

  const renderIconCopy = () => {
    return (
      <Button className='btn-copy-content' onClick={handleCopy}>
        <span className='icon-content_copy'></span>
      </Button>
    );
  };

  return (
    <>
      {renderLeagueHomeAway()}
      <p className='match-code'>{MatchCode}</p>
      <div className='list-button'>
        {Boolean(AdminSiteId) && renderLinkIcon()}
        {renderIconCopy()}
      </div>
    </>
  );
};

export default MatchContainLinkDetailCell;
