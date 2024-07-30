import CheckBoxFilter from './checkBox/CheckBoxFilter';
import FilterTicketCenter from './filter/FilterTicketCenter';
import CreateTicketButton from '../../../Shared/createTicketNew/CreateTicketButton';
import MarkAllReadButton from './button/MarkAllReadButton';
import store from '../../app/storeNative';
import { MARKET, STATUS } from '../../const/const.json';
import { BUTTON_TYPE } from '../../../Shared/createTicketNew/const/const';

const { MY_TICKET } = BUTTON_TYPE;

const Header = () => {
  const { permission } = store.getState().pagePolicy;
  const { ActorId, HasWrite } = permission;
  const defaultValue = {
    MarketKey: MARKET[0].value,
    ActorId,
    TicketStatuses: STATUS[0].value
  };

  const renderFilterTicketCenter = () => (
    <div className='filter-control '>
      <FilterTicketCenter defaultValue={defaultValue} />
    </div>
  );

  return (
    <div className='block-filter has-dropdown'>
      {renderFilterTicketCenter()}
      {HasWrite && (
        <CreateTicketButton buttonType={MY_TICKET} isCreateLogWork />
      )}

      <MarkAllReadButton actorId={ActorId} filterModel></MarkAllReadButton>

      <div className='btn-right'>
        <CheckBoxFilter />
      </div>
    </div>
  );
};

export default Header;
