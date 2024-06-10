import { PureComponent } from 'react';
import { Button } from 'reactstrap';
import { v4 as uuidv4 } from 'uuid';
import { PlusIcon, ArrowRightIcon } from 'mdi-react';

import PropTypes from 'prop-types';
import EventPopup from '../../Shared/EventPopup';
import MatchInfo from '../components/MatchInfo';

const propTypesEvent = {
  isAllowAddEvent: PropTypes.bool.isRequired,
  policy: PropTypes.object.isRequired,
  addSpecialEvent: PropTypes.func.isRequired,
  openDetailInformation: PropTypes.func.isRequired,
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  detailInformationPolicy: PropTypes.object.isRequired
};

class EventCell extends PureComponent {
  render() {
    const {
      isAllowAddEvent,
      policy,
      addSpecialEvent,
      openDetailInformation,
      data,
      rowIndex,
      detailInformationPolicy
    } = this.props;
    const event = data[rowIndex];

    const allowToAddMinorMatch =
      isAllowAddEvent &&
      policy.FullBetTypes.HasWrite &&
      event.OWPMatchId === event.ParentMatchId;

    const dataLength = data.length;
    const threeLastRow = [dataLength - 1, dataLength - 2, dataLength - 3];
    const className = threeLastRow.some(i => i === rowIndex)
      ? 'last-popup'
      : null;
    const handleCopy = () => {
      const kickOffTime = `K.O Time: ${event.KickOffTime}`;
      const league = `League: ${event.League}`;
      const match = `Match: ${event.Home} -vs- ${event.Away}`;
      const matchNo = `Match ID: ${event.AdminSiteId ?? ''}`;
      const iptv = `TV/IPTV: ${event.IPTVCopy ?? ''} `;
      const provider = `Provider: ${event.Provider ?? ''}`;

      window.executeCopy(kickOffTime, league, match, matchNo, iptv, provider);
    };
    return (
      <div className='table-cell' key={`Event${uuidv4()}${rowIndex}`}>
        {event.LeagueGroup && (
          <h4 className='league-group league-badge'>{event.LeagueGroup}</h4>
        )}
        <h4 className='league-group league-name'>{event.League}</h4>

        {allowToAddMinorMatch && (
          <a
            onClick={e => {
              e.preventDefault();
              addSpecialEvent(event);
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
              openDetailInformation(event.OWPMatchId);
            }}
            className='link-livedetail'>
            <ArrowRightIcon size={13} />
          </a>
        )}

        <MatchInfo event={event} componentId={uuidv4()} />

        {event.AdminSiteId > 0 && (
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
            />
          </>
        )}
        <>
          <Button className='btn-copy-content' onClick={handleCopy}>
            <span className='icon-content_copy'></span>
          </Button>
        </>
      </div>
    );
  }
}

EventCell.propTypes = propTypesEvent;

export default EventCell;
