import { useContext } from 'react';
import { Button } from 'reactstrap';
import { CellProps } from '../../../type/TableType';
import EventPopup from '../../../../Shared/EventPopup';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { openPopupDetail } from '../../tableSlice/tableSlice';
import { two, three } from '../../../LiveGameListConst.json';
import { AppContext } from '../../../../Shared/Context';

const MatchCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const {
    Home,
    Away,
    IsAllowedToViewDetail,
    MatchCode,
    AdminSiteId,
    OWPMatchId,
    IsFantasy,
    League,
    KickOffTimeDisplay,
    IPTVCopy,
    Provider,
    MinorBettypeId
  } = dataSource[rowIndex];

  const dispatch = useStableDispatch();
  const dataLength = dataSource.length;
  const threeLastRow = [dataLength - 1, dataLength - two, dataLength - three];
  const { detailInformationPolicy } = useContext(AppContext);

  const className = threeLastRow.some(i => i === rowIndex)
    ? 'last-popup'
    : null;

  const handleOpenLiveDetail = () => {
    detailInformationPolicy.HasRead &&
      IsAllowedToViewDetail &&
      dispatch(openPopupDetail(OWPMatchId));
  };

  const handleToCopy = () => {
    const kickOffTime = `K.O Time: ${KickOffTimeDisplay}`;
    const league = `League: ${League}`;
    const match = `Match: ${Home} -vs- ${Away}`;
    const matchNo = `Match ID: ${AdminSiteId ?? ''}`;
    const iptv = `TV/IPTV: ${IPTVCopy ?? ''} `;
    const provider = `Provider: ${Provider ?? ''}`;

    window.executeCopy(kickOffTime, league, match, matchNo, iptv, provider);
  };

  return (
    <>
      <div className='match-cell'>
        <div onClick={handleOpenLiveDetail} className='open-live-detail'>
          <p className='home-team'>{Home}</p>
          <p className='away-team'>{Away}</p>
        </div>
        {MatchCode && <p className='match-code'> {MatchCode}</p>}
        <div className='list-button'>
          {AdminSiteId > 0 && (
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
          )}
          <>
            <Button className='btn-copy-content' onClick={handleToCopy}>
              <span className='icon-content_copy'></span>
            </Button>
          </>
        </div>
      </div>
    </>
  );
};

export default MatchCell;
