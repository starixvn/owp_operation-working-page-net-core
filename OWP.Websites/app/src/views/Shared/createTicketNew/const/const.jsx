import { netCoreURL, netFwURL } from '../../../TicketCenter/const/const.json';
import {
  NET_CORE_QUERY_SIMPLE_API,
  QUERY_SIMPLE_API,
  NET_CORE_FILE_UPLOAD,
  NET_FW_FILE_UPLOAD
} from '../../../Shared/Constants';
const URL = {
  ADD_NEW_REQUEST_SUPPORT: 'new-ticket-issue-support',
  ADD_NEW_REQUEST_OTHER: 'new-ticket-issue-other',
  ADD_NEW_REQUEST_EVENT: 'new-ticket-issue-event',
  ADD_NEW_REQUEST_LEAGUE: 'new-ticket-issue-league',
  ADD_NEW_REQUEST_LOG_WORK: 'new-ticket-issue-log-work',
  ADD_NEW_REQUEST_LIVE_INCIDENT: 'new-ticket-issue-live-incident',
  GET_SUBTYPE: 'get-subtype',
  GET_EVENT: 'new-ticket/get-event',
  GET_EVENT_LGA: 'new-ticket/get-event-lga',
  UPLOAD_MULTIPLE: 'multiple-upload',
  GET_ASSIGNEES_LGA: 'get-assignees-lga',
  GET_MATCH_INFO: 'get-match-info'
};
const BUTTON_TYPE = {
  LIVE_GAME: 'live-game',
  PRE_MATCH: 'pre-match',
  MY_TICKET: 'my-ticket'
};
const RECIPIENT_TYPE = {
  INDIVIDUAL: 1,
  GROUP: 2
};
const buildRequestURL = key =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${key}`
    : `${netFwURL}/${key}`;

const urlGetSubType = buildRequestURL(URL.GET_SUBTYPE);
const urlRequestSupport = buildRequestURL(URL.ADD_NEW_REQUEST_SUPPORT);
const urlRequestOther = buildRequestURL(URL.ADD_NEW_REQUEST_OTHER);
const urlRequestEvent = buildRequestURL(URL.ADD_NEW_REQUEST_EVENT);
const urlRequestLeague = buildRequestURL(URL.ADD_NEW_REQUEST_LEAGUE);
const urlRequestLogWork = buildRequestURL(URL.ADD_NEW_REQUEST_LOG_WORK);
const urlRequestLiveIncident = buildRequestURL(
  URL.ADD_NEW_REQUEST_LIVE_INCIDENT
);
const urlGetMatchInfo = buildRequestURL(URL.GET_MATCH_INFO);

const urlGetAssigneesLGA = (() => {
  const netCoreUrlEvent = 'api-netcore/TicketEvent';
  const netFwURLEvent = 'api/TicketEvent';

  return process.env.NODE_ENV === 'development'
    ? `${netCoreUrlEvent}/${URL.GET_ASSIGNEES_LGA}`
    : `${netFwURLEvent}/${URL.GET_ASSIGNEES_LGA}`;
})();
const urlGetEvent = buildRequestURL(URL.GET_EVENT);
const urlGetEventLGA = buildRequestURL(URL.GET_EVENT_LGA);
const urlUploadFile =
  process.env.NODE_ENV === 'development'
    ? `${NET_CORE_FILE_UPLOAD}/${URL.UPLOAD_MULTIPLE}`
    : `${NET_FW_FILE_UPLOAD}/${URL.UPLOAD_MULTIPLE}`;

const urlGetRecipient =
  process.env.NODE_ENV === 'development'
    ? `${NET_CORE_QUERY_SIMPLE_API}`
    : `${QUERY_SIMPLE_API}`;

export {
  URL,
  urlGetSubType,
  urlRequestSupport,
  urlRequestLogWork,
  urlRequestOther,
  urlRequestEvent,
  urlRequestLeague,
  urlRequestLiveIncident,
  urlUploadFile,
  urlGetEvent,
  urlGetRecipient,
  urlGetEventLGA,
  urlGetMatchInfo,
  RECIPIENT_TYPE,
  urlGetAssigneesLGA,
  BUTTON_TYPE
};
