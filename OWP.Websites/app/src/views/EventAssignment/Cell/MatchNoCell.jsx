import { PureComponent } from 'react';
import { MatchStatuses } from '../../Shared/Constants';
import { UncontrolledTooltip } from 'reactstrap';
import { LinkIcon, LinkOffIcon } from 'mdi-react';
import AbsencePersonIcon from '../../../assets/svg-element/AbsencePersonIcon';
import PropTypes from 'prop-types';
import { isEqual } from 'lodash';

class MatchNoCell extends PureComponent {
  constructor(props) {
    super(props);
    this.renderInfoSiteLinked = this.renderInfoSiteLinked.bind(this);
  }

  renderInfoSiteLinked(event) {
    if (this.isLiveStatusSynchronized(event)) {
      return <span className='badge-label match-status'>LIVE</span>;
    }

    return event.IsInfoSiteLinked ? (
      <LinkIcon color='#00920A' size={15} />
    ) : (
      <LinkOffIcon color='#FF0000' size={15} />
    );
  }

  isLiveStatusSynchronized(event) {
    const isLive =
      isEqual(event.MatchStatus, MatchStatuses.InPlay) ||
      isEqual(event.MatchStatus, MatchStatuses.Live) ||
      isEqual(event.MatchStatus, MatchStatuses.Postponed);
    return event.IsLiveStatusSynchronized && isLive;
  }

  render() {
    const { data, rowIndex } = this.props;
    const event = data[rowIndex];
    return (
      <div className='match-no'>
        {event.IsShowAbsenceIcon && <AbsencePersonIcon />}
        <span className='match-number'>{event.MatchNo}</span>
        <span id={`InfoSite${rowIndex}`} className='icon-validInfoSiteId'>
          {this.renderInfoSiteLinked(event)}
        </span>
        <UncontrolledTooltip placement='right' target={`InfoSite${rowIndex}`}>
          InfositeId: {event.InfoSiteMatchId}
        </UncontrolledTooltip>

        {!isEqual(event.MatchStatus, MatchStatuses.Live) && (
          <>
            <span
              id={`MatchStatus${rowIndex}`}
              className={`icon-status icon-${event.MatchStatus.toLowerCase()}`}
            />
            <UncontrolledTooltip
              placement='right'
              target={`MatchStatus${rowIndex}`}>
              {event.MatchStatus}
            </UncontrolledTooltip>
          </>
        )}
      </div>
    );
  }
}

MatchNoCell.propTypes = {
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default MatchNoCell;
