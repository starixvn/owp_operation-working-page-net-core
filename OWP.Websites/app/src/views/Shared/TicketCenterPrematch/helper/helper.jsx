import { EVENT_TYPE, URL, TICKET_TYPE } from '../const/const.json';
import { FunctionKeys } from '../../Constants';
import getPermission from '../../permission/getPermission';

const { LEAGUE, MATCH, OTHER, SUPPORT, LIVE_INCIDENT, LOG_WORK } = EVENT_TYPE;
const {
  GET_TICKET_DETAIL_LEAGUE,
  GET_TICKET_DETAIL_MATCH,
  GET_TICKET_DETAIL_OTHER,
  GET_TICKET_DETAIL_SUPPORT,
  GET_TICKET_DETAIL_LIVE_INCIDENT,
  GET_TICKET_DETAIL_LOG_WORK
} = URL;

const getDetailSegmentUrlByEventType = eventType => {
  const detailSegmentUrlLookupTable = {
    [LEAGUE]: GET_TICKET_DETAIL_LEAGUE,
    [MATCH]: GET_TICKET_DETAIL_MATCH,
    [OTHER]: GET_TICKET_DETAIL_OTHER,
    [SUPPORT]: GET_TICKET_DETAIL_SUPPORT,
    [LIVE_INCIDENT]: GET_TICKET_DETAIL_LIVE_INCIDENT,
    [LOG_WORK]: GET_TICKET_DETAIL_LOG_WORK
  };
  return detailSegmentUrlLookupTable[eventType];
};

const buildGoToMyTicketLink = () => (!process.env.NODE_ENV || process.env.NODE_ENV === 'development')
? ''
: '/MyTickets';

const getMyTicketPermission = () => getPermission(FunctionKeys.Tickets);

export {
  getDetailSegmentUrlByEventType,
  EVENT_TYPE,
  URL,
  TICKET_TYPE,
  buildGoToMyTicketLink,
  getMyTicketPermission
};
