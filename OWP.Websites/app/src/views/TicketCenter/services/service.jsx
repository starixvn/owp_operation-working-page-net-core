import { get } from '../../Shared/Service/Service';
import { getTicketType, URL, updateTicketsAsRead } from '../const/const.json';
import { netCoreURLTicketCenter } from '../const/apiUrl';

const getTicketTypeOption = async () => {
  const url = `${netCoreURLTicketCenter}/${getTicketType}`;

  const response = await get({ url });
  return response.data;
};

const getStatusHistory = async ticketId => {
  const url = `${netCoreURLTicketCenter}/${URL.GET_STATUS_HISTORY}`;

  const response = await get({ url, ticketId });
  return response.data;
};

const markAllTicketsAsRead = async actorId => {
  const url = `${netCoreURLTicketCenter}/${updateTicketsAsRead}`;

  const response = await get({ url, actorId });
  return response;
};

export { getTicketTypeOption, getStatusHistory, markAllTicketsAsRead };
