import { get } from '../../Shared/Service/Service';
import {
  netCoreURL,
  getTicketType,
  URL,
  updateTicketsAsRead
} from '../const/const.json';

const getTicketTypeOption = async () => {
  const url = `${netCoreURL}/${getTicketType}`;

  const response = await get({ url });
  return response.data;
};

const getStatusHistory = async ticketId => {
  const url = `${netCoreURL}/${URL.GET_STATUS_HISTORY}`;

  const response = await get({ url, ticketId });
  return response.data;
};

const markAllTicketsAsRead = async actorId => {
  const url = `${netCoreURL}/${updateTicketsAsRead}`;

  const response = await get({ url, actorId });
  return response;
};

export { getTicketTypeOption, getStatusHistory, markAllTicketsAsRead };
