import { get } from '../../Shared/Service/Service';
import { netCoreURL, netFwURL } from '../const/const.json';

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

export { getStatusHistory, markAllTicketsAsRead };
