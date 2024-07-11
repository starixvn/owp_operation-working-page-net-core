import { OWPPage, OWPHeader, OWPBody } from '../Shared/common/OWP/OPWPage';
import FilterWrapper from './features/header/filter/FilterNative';
import { FunctionKeys } from '../Shared/Constants';
import TicketList from './features/table/TicketList';
import usePermission from '../Shared/customHook/usePermission';
import Loading from '../components/Loading';

const SmallTicketCenterManagementNative = () => {
  const { permission } = usePermission(FunctionKeys.TicketCenter, 'pagePolicy');
  if (!permission.Username) {
    return <Loading></Loading>;
  }
  return (
    <div className='small__ticket-center'>
      <OWPPage className='Event-Tickets'>
        <OWPHeader className='tickets-tab'>
          <FilterWrapper />
        </OWPHeader>
        <OWPBody className='tickets-tab__body'>
          <TicketList />
        </OWPBody>
      </OWPPage>
    </div>
  );
};

export default SmallTicketCenterManagementNative;
