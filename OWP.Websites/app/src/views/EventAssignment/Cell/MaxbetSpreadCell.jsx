import { PureComponent } from 'react';
import ReactTooltip from 'react-tooltip';
import { v4 as uuidv4 } from 'uuid';
import PropTypes from 'prop-types';

class MaxbetSpreadCell extends PureComponent {
  constructor(props) {
    super(props);
    this.renderMaxbetSpreadMbc = this.renderMaxbetSpreadMbc.bind(this);
  }

  renderMaxbetSpreadMbc(HandicapSetting, OverUnderSetting, uuidKey) {
    const isDisplayHDPSetting = this.isDisplayHDPSetting(
      HandicapSetting,
      OverUnderSetting
    );
    const isHDPSpreadExisting =
      HandicapSetting && HandicapSetting.SpreadToDisplay;

    return (
      <div>
        <span data-for={uuidKey} data-tip='Maxbet/Spread of MBC'>
          {HandicapSetting && HandicapSetting.MaxBetToDisplay}
          {isDisplayHDPSetting && (
            <span>({HandicapSetting && HandicapSetting.SpreadToDisplay}</span>
          )}
          {OverUnderSetting && OverUnderSetting.SpreadToDisplay && (
            <>
              {isHDPSpreadExisting && <span>-</span>}
              {OverUnderSetting.SpreadToDisplay}
            </>
          )}
          {isDisplayHDPSetting && <span>)</span>}
        </span>
        <ReactTooltip place='right' id={uuidKey} effect='solid' />
      </div>
    );
  }

  isDisplayHDPSetting(HandicapSetting, OverUnderSetting) {
    return (
      (HandicapSetting && HandicapSetting.SpreadToDisplay) ||
      (OverUnderSetting && OverUnderSetting.SpreadToDisplay)
    );
  }

  render() {
    const { data, rowIndex } = this.props;
    const { OWPMatchId, HandicapSetting, OverUnderSetting } = data[rowIndex];
    const uuidKey = `${OWPMatchId}_${uuidv4()}`;
    return (
      <div className='maxbet-cell'>
        {this.renderMaxbetSpreadMbc(HandicapSetting, OverUnderSetting, uuidKey)}
      </div>
    );
  }
}

const propTypesBase = {
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

MaxbetSpreadCell.propTypes = propTypesBase;

export default MaxbetSpreadCell;
