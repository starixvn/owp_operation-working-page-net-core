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
      : `${netFwURL}/${getTicketType}`;

  const response = await get({ url });
  return response.data;
};

const getStatusHistory = async ticketId => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${netCoreURL}/${URL.GET_STATUS_HISTORY}`
      : `${netFwURL}/${URL.GET_STATUS_HISTORY}`;

  const response = await get({ url, ticketId });
  return response.data;
};

const markAllTicketsAsRead = async actorId => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${netCoreURL}/${updateTicketsAsRead}`
      : `${netFwURL}/${updateTicketsAsRead}`;

  const response = await get({ url, actorId });
  return response;
};

export { getTicketTypeOption, getStatusHistory, markAllTicketsAsRead };
