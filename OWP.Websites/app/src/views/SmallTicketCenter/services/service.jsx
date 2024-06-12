import { get } from '../../Shared/Service/Service';
import { netCoreURL } from '../const/const.json';

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

export { getStatusHistory, markAllTicketsAsRead };
