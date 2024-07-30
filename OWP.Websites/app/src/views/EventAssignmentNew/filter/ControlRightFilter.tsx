import GoToMyTicket from '../../Shared/TicketCenterPrematch/GoToMyTicket';
import DailyWorking from './DailyWorking';
import { useAppSelector } from '../app/hooks';
import dayjs from 'dayjs';

const ControlRightFilter = () => {
  const syncOffVRByUser = useAppSelector(
    state => state.eventAssignmentTable.syncOffVRByUser
  );
  const syncOffVRLastTime = useAppSelector(
    state => state.eventAssignmentTable.syncOffVRLastTime
  );
  const dateUpdated = useAppSelector(
    state => state.eventAssignmentTable.dateUpdated
  );
  const lastUpdated = dateUpdated
    ? dayjs(dateUpdated.substring(0, dateUpdated.length - 6)).format(
        'MMM DD, hh:mm A'
      )
    : '';
  return (
    <div className='filter-control-right'>
      <div className='top-info'>
        <span className='sync-off-vr-last-update'>
          Sync Off VR by {syncOffVRByUser}: {syncOffVRLastTime}
        </span>
        <span className='lastupdate'>| Last Updated: {lastUpdated}</span>
        <GoToMyTicket />
      </div>
      <DailyWorking />
    </div>
  );
};
export default ControlRightFilter;
