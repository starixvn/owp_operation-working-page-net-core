import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import TimeOfMatch from '../../Shared/LiveGameShared/TimeOfMatch';
class KOTimeCell extends PureComponent {
  render() {
    const { data, rowIndex } = this.props;
    const {
      OtherTime,
      OtherTimeStr,
      KickOffTime,
      KickOffTimeStr,
      InfoSiteTime,
      InfoSiteTimeDisplay
    } = data[rowIndex];
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
  }
}

KOTimeCell.propTypes = {
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

export default KOTimeCell;
