import store from '../app/storeNative';
import { handleReloadTicketDetail } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const handleChangeSubtype = () => {
  handleReloadTicketDetail(store);
};

export { handleChangeSubtype };
