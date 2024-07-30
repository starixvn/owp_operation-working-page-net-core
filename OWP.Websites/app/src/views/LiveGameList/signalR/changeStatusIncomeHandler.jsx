import store from '../app/storeNative';
import { handleReloadTicketDetail } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const handleChangeStatus = () => {
  handleReloadTicketDetail(store);
};

export { handleChangeStatus };
