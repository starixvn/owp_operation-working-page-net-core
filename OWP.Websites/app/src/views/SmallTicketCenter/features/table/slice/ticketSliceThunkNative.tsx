import { post } from '../../../../Shared/Service/Service';
import Utils from '../../../../Shared/Utils';
import { createAppAsyncThunk } from '../../../app/hooksNative';

import {
  netCoreURL,
  netFwURL,
  getEventTicket
} from '../../../const/const.json';
import { TTicket, TFilerParam } from '../../../type';

const genUrl = (routePostfix: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${routePostfix}`
    : `${window.netCoreRootUrl}/${netFwURL}/${routePostfix}`;

const getEventTickets = createAppAsyncThunk<Array<TTicket>, void>(
  'TicketCenter/getEventChecking',
  async (_, { getState }) => {
    const {
      marketKey,
      fromDateFilter,
      toDateFilter,
      keyword,
      ticketTypeId,
      ticketStatuses
    } = getState().filter.entities.filterInfo;
    const userId = getState().pagePolicy.permission.ActorId;

    const filterParam = {
      actorId: userId,
      marketKey,
      fromDateFilter: Utils.formatDate(fromDateFilter),
      toDateFilter: Utils.formatDate(toDateFilter),
      keyword,
      ticketTypeId,
      ticketStatuses
    };
    const url = genUrl(getEventTicket);
    const response = await post({ url, ...filterParam });
    return response.data;
  }
);

export { getEventTickets };
