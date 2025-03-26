import Axios from 'axios';
import {
  urlGetEvent,
  urlGetEventLGA,
  urlGetRecipient,
  urlGetSubType,
  urlGetAssigneesLGA,
  urlGetMatchInfo
} from './const/const';
import { orderBy } from 'lodash';
import { TICKET_TYPE } from '../TicketCenterPrematch/const/const.json';
import { post } from '../Service/Service';
import { NET_CORE_QUERY_SIMPLE_API } from '../Constants';

const { EVENT_CHECKING } = TICKET_TYPE;

const recipient = async keyword => {
  const getModel = {
    keyword,
    includeGroup: 'true',
    inCludeSection: 'true',
    apiKey: 'ticket-center-get-recipient'
  };
  const { data } = await Axios.post(urlGetRecipient, getModel);
  const result = orderBy(
    data,
    ['recipientType', ({ groupName, userName }) => groupName || userName],
    ['desc', 'asc']
  );
  return result;
};

const recipientBySubType = async subTypeId => {
  const getModel = {
    subTypeId,
    apiKey: 'ticket-center-get-default-recipient'
  };
  return await Axios.post(urlGetRecipient, getModel);
};

const event = async (valueSearch, type) => {
  const url = type === EVENT_CHECKING ? urlGetEvent : urlGetEventLGA;
  const result = await Axios.get(url, {
    params: { keyword: valueSearch }
  });
  return result;
};

const getSubType = async ticketType => {
  const model = {
    TicketTypeId: ticketType
  };
  const result = await Axios.post(urlGetSubType, model);
  return result;
};

const queryAssigneesByOwpMatchId = async model => {
  const response = await Axios.get(urlGetAssigneesLGA, {
    params: model
  });
  return response.data;
};

const queryAssigneesByParentMatchId = async parentId => {
  const response = await post({
    url: NET_CORE_QUERY_SIMPLE_API,
    apiKey: 'group-chat-query-recipients',
    parentId
  });
  return response.data;
};

const getMatchInfoByAdminSiteId = async adminSiteId => {
  const response = await Axios.get(urlGetMatchInfo, {
    params: adminSiteId
  });
  return response.data;
};

const Service = {
  recipient,
  recipientBySubType,
  event,
  getSubType,
  queryAssigneesByOwpMatchId,
  getMatchInfoByAdminSiteId,
  queryAssigneesByParentMatchId
};

export default Service;
