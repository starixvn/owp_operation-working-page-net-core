import { get } from '../../Shared/Service/Service';
import {
  netCoreURL,
  netFwURL,
  getTicketType,
  URL,
  updateTicketsAsRead
} from '../const/const.json';

const getTicketTypeOption = async () => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${netCoreURL}/${getTicketType}`
      : `${window.netCoreRootUrl}/${netFwURL}/${getTicketType}`;

  const response = await get({ url });
  return response.data;
};

const getStatusHistory = async ticketId => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${netCoreURL}/${URL.GET_STATUS_HISTORY}`
      : `${window.netCoreRootUrl}/${netFwURL}/${URL.GET_STATUS_HISTORY}`;

  const response = await get({ url, ticketId });
  return response.data;
};

const markAllTicketsAsRead = async actorId => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${netCoreURL}/${updateTicketsAsRead}`
      : `${window.netCoreRootUrl}/${netFwURL}/${updateTicketsAsRead}`;

  const response = await get({ url, actorId });
  return response;
};

export { getTicketTypeOption, getStatusHistory, markAllTicketsAsRead };
