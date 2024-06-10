import { Component } from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import Animated from 'react-select/animated';
import {
  bettypeIdMainMatch,
  allBettype,
  bettypeIdDetectNoOfCornersTotalBooking,
  bettypeIdNoOfCornersTotalBooking,
  bettypeIdCommon,
  bettypeIdXminOfNoOfCornersTotalBooking,
  bettypeIdXmins
} from '../../Shared/Constants';
import { isNil } from 'lodash';
import memoize from 'memoize-one';

const buildBettypeOptions = minorBettypeId => {
  if (isNil(minorBettypeId)) {
    return allBettype.filter(bettype =>
      bettypeIdMainMatch.some(id => bettype.value === id)
    );
  }
  if (
    bettypeIdDetectNoOfCornersTotalBooking.some(id => id === minorBettypeId)
  ) {
    return allBettype.filter(bettype =>
      bettypeIdNoOfCornersTotalBooking.some(id => bettype.value === id)
    );
  }
  return allBettype.filter(bettype =>
    bettypeIdCommon.some(id => bettype.value === id)
  );
};

const getBettypeValue = bettypeId => {
  let currentBettypeId = bettypeId;
  if (bettypeIdXminOfNoOfCornersTotalBooking.some(b => b === bettypeId)) {
    currentBettypeId = bettypeIdXmins;
  }
  return allBettype.find(bettype => bettype.value === currentBettypeId);
};

const propTypes = {
  minorBettypeId: PropTypes.number,
  BettypeId: PropTypes.number,
  isDisabled: PropTypes.bool,
  handleBettypeChange: PropTypes.func.isRequired
};

class BettypeCell extends Component {
  reBuildBettypeOptions = memoize(minorBettypeId =>
    buildBettypeOptions(minorBettypeId)
  );

  render() {
    const {
      isDisabled,
      handleBettypeChange,
      minorBettypeId,
      BettypeId
    } = this.props;
    const [options, value] = [
      this.reBuildBettypeOptions(minorBettypeId),
      getBettypeValue(BettypeId)
    ];
    return (
      <td>
        <div className='content select-up'>
          <Select
            name='bettype'
            components={Animated}
            options={options}
            value={value}
            isDisabled={isDisabled}
            className={
              'basic-multi-select' + (isNil(BettypeId) ? ' input-invalid' : '')
            }
            classNamePrefix='select'
            menuPlacement='bottom'
            onChange={
              selectedBettype =>
                handleBettypeChange(selectedBettype.value)
            }
          />
        </div>
      </td>
    );
  }
}
BettypeCell.propTypes = propTypes;
export default BettypeCell;
