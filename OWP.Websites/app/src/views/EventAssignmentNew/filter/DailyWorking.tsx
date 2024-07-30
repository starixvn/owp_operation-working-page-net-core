import CreateTicketButton from '../../Shared/createTicketNew/CreateTicketButton';
import { BUTTON_TYPE } from '../../Shared/createTicketNew/const/const';
import { useAppSelector } from '../app/hooks';
import Utils from '../../Shared/Utils';

const { LIVE_GAME } = BUTTON_TYPE;

const DailyWorking = () => {
  const policy = useAppSelector(state => state.policy);
  const selectedDate = useAppSelector(
    state => state.filter.filterState.selectedDate
  );
  const dailyWorkingShiftPolicy = policy.entities.dailyWorkingShiftPolicy;
  const ticketHasWrite = policy.entities.MyTicket.HasWrite;
  const openDailyWS = () => {
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
    popup?.focus();
  };

  return (
    <div className='filter-control btn-block btn-block--right'>
      {ticketHasWrite && <CreateTicketButton buttonType={LIVE_GAME} />}
      {(dailyWorkingShiftPolicy.HasRead ||
        dailyWorkingShiftPolicy.HasWrite) && (
        <button className='button-working-shift' onClick={openDailyWS}>
          Daily Working
        </button>
      )}
    </div>
  );
};
export default DailyWorking;
