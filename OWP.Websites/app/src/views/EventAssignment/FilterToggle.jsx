import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import Utils from '../Shared/Utils';
import { FunctionKeys } from '../Shared/Constants';
import { ColumnFullBetTypes, ColumnPHBetTypes } from './constants/Column';
import CreateTicketButton from '../Shared/createTicketNew/CreateTicketButton';
import ToggleColumn from '../Shared/ShowHideColumn/ToggleColumn';
import { BUTTON_TYPE } from '../Shared/createTicketNew/const/const';

const { LIVE_GAME } = BUTTON_TYPE;

const propTypes = {
  onSelected: PropTypes.func,
  selectedDate: PropTypes.object,
  policy: PropTypes.object,
  dailyWorkingShiftPolicy: PropTypes.object
};

class FilterToggle extends PureComponent {
  onSelected = e => {
    this.props.onSelected(e);
  };

  openDailyWS() {
    const { selectedDate } = this.props;
    const url = `DailyWorkingShift?date=${Utils.formatDate(selectedDate)}`;
    const width = window.innerWidth * 0.8;
    const height = window.innerHeight * 0.8;
    const top = window.screenY + (window.innerHeight - height) / 2;
    const left = window.screenX + (window.innerWidth - width) / 2;
    const popup = window.open(
      url,
      '',
      `width = ${width}, height = ${height}, top = ${top}, left = ${left}`
    );
    popup.focus();
  }

  render() {
    const { policy, dailyWorkingShiftPolicy } = this.props;
    const ticketHasWrite = policy.MyTicket?.HasWrite;
    const displayColumn = policy.FullBetTypes.HasRead
      ? ColumnFullBetTypes
      : ColumnPHBetTypes;
    const storageKey = policy.FullBetTypes.HasRead
      ? FunctionKeys.LiveGameAssignment_FullBetTypes
      : FunctionKeys.LiveGameAssignment_PHBetTypes;

    const localStorageKey = `localStorageKey_ShowHideColumn_${policy.Username}_${storageKey}`;
    return (
      <div className='filter-control btn-block btn-block--right'>
        {ticketHasWrite && <CreateTicketButton buttonType={LIVE_GAME} />}
        {(dailyWorkingShiftPolicy.HasRead ||
          dailyWorkingShiftPolicy.HasWrite) && (
          <button
            className='button-working-shift'
            onClick={() => this.openDailyWS()}>
            Daily Working
          </button>
        )}
        {policy && policy.Username && (
          <ToggleColumn
            onSelected={e => this.onSelected(e)}
            options={displayColumn}
            localStorageKey={localStorageKey}
          />
        )}
      </div>
    );
  }
}

FilterToggle.propTypes = propTypes;
export default FilterToggle;
