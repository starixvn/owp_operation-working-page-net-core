import store from '../app/store';
import { handleReloadTicketDetail } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const handleChangeSubtype = () => {
  handleReloadTicketDetail(store);
};

export { handleChangeSubtype };
