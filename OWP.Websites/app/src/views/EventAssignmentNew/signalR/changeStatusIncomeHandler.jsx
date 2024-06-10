import store from '../app/store';
import { handleReloadTicketDetail } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const handleChangeStatus = () => {
  handleReloadTicketDetail(store);
};

export { handleChangeStatus };
