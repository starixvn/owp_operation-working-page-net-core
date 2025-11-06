import {
  NET_CORE_QUERY_SIMPLE_API,
  NET_CORE_UPDATE_SIMPLE_API
} from '../../Shared/Constants';

const url = `/api/PreMatchAssignment`;

const simpleApiQueryUrl = `${NET_CORE_QUERY_SIMPLE_API}`;

const simpleApiUpdateUrl = `${NET_CORE_UPDATE_SIMPLE_API}`;

const URL_QUERY_CHANGE_LOG = `${url}/get-log`;

const URL_QUERY_WORKING_SHIFT = `${url}/get-working-shift`;

const URL_QUERY_INFO_MATCH = `${url}/get-info-match`;
const URL_QUERY_ASSIGNEE = `${url}/get-assignee-pre-match`;
const URL_NIGHT_SHIFT = `${url}/get-night-shift`;
const URL_DAY_SHIFT = `${url}/get-day-shift`;
const URL_CREATE_DIRECTION_TICKET = `${url}/create-rc-discussion`;
const URL_QUERY_LEAGUE_BY_ID = `${url}/query-league-by-id`;
const URL_QUERY_ASSIGNED_LEAGUE_ID = `${url}/get-assigned-league-id`;
const URL_QUERY_TICKET_LIST = `${url}/get-ticket-list`;

const LAST_UPDATED = {
  apiKey: 'pre-match-assignment-get-last-updated'
};

const TAKE_OVER_API_KEY = 'pre-match-assignment-get-start-time';

const QUERY_DELETE_LEAGUE_API_KEY = 'pre-match-assignment-get-deleted-leagues';

const QUERY_DELETE_LEAGUE_RECOVER_API_KEY = 'pre-match-assignment-recover';

const QUERY_CHECK_LEAGUE_CONTAINS_ASSIGNMENT =
  'pre-match-assignment-check-league-contains-assignment';

const URL = {
  INSERT_NOTE: `${url}/post-note-message`,
  CREATE_NOTE: `${url}/create-note`,
  UPDATE_NOTE: `${url}/edit-note-message`,
  SAVE_PREMATCH_ASSIGNMENT: `${url}/save`
};

const API_KEY = {
  QUERY_NOTE: 'pre-match-assignment-get-note-messages',
  DELETE_NOTE: 'pre-match-assignment-delete-note-messages'
};

const NOTE_TYPE = {
  LEAGUE: 'LEAGUE',
  MATCH: 'MATCH'
};

const ticketType = {
  EVEN_CHECKING: 1,
  TICKET_SUPPORT: 3
};

const MATCH_OFFER = [
  { value: 'Opened', label: 'Running' },
  { value: 'HDP (?)', label: 'HDP (?)' },
  { value: 'OU (?)', label: 'OU (?)' },
  { value: '', label: 'Waiting' }
];

const EARLY_OFFERED = [
  { value: 1, label: 'Offered' },
  { value: 0, label: 'Have not Offered' }
];

const TradingPageUrl = 'http://deadball.oneops.net/Market/totalbet.aspx';

const BetType = {
  HDP: 1,
  OU: 3
};
const localStoreKeyTime = {
  day: 'DayShift-PreMatchTimeCondition',
  night: 'NightShift-PreMatchTimeCondition'
};

const BETTYPE = {
  HDP: 1,
  OU: 3
};

const ASSIGNMENT_TYPE = {
  MAIN: 1,
  OTHER: 2
};

const GHOST_OPTIONS = [
  { value: 'Order', label: 'Order' },
  { value: 'Ghost 1', label: 'Ghost 1' },
  { value: 'Ghost 2', label: 'Ghost 2' },
  { value: 'Ghost 3', label: 'Ghost 3' },
  { value: 'Outside', label: 'Outside' },
  { value: 'Danger', label: 'Danger' },
  { value: 'Other', label: 'Other' }
];

const BETTYPE_OPTIONS = [
  { value: 1, label: 'FT HDP' },
  { value: 3, label: 'FT OU' },
  { value: 7, label: 'HT HDP' },
  { value: 8, label: ' HT OU' }
];

const BETON_OPTIONS = [
  [
    { value: 'Home', label: 'Home' },
    { value: 'Away', label: 'Away' }
  ],
  [
    { value: 'Over', label: 'Over' },
    { value: 'Under', label: 'Under' }
  ]
];
export {
  EARLY_OFFERED,
  ticketType,
  URL_QUERY_CHANGE_LOG,
  URL_QUERY_WORKING_SHIFT,
  URL_QUERY_INFO_MATCH,
  URL_QUERY_ASSIGNEE,
  URL_CREATE_DIRECTION_TICKET,
  QUERY_DELETE_LEAGUE_API_KEY,
  QUERY_DELETE_LEAGUE_RECOVER_API_KEY,
  QUERY_CHECK_LEAGUE_CONTAINS_ASSIGNMENT,
  LAST_UPDATED,
  TAKE_OVER_API_KEY,
  URL_NIGHT_SHIFT,
  URL_DAY_SHIFT,
  URL_QUERY_LEAGUE_BY_ID,
  API_KEY,
  URL,
  NOTE_TYPE,
  MATCH_OFFER,
  TradingPageUrl,
  BetType,
  URL_QUERY_ASSIGNED_LEAGUE_ID,
  simpleApiQueryUrl,
  simpleApiUpdateUrl,
  URL_QUERY_TICKET_LIST,
  localStoreKeyTime,
  BETTYPE,
  ASSIGNMENT_TYPE,
  GHOST_OPTIONS,
  BETTYPE_OPTIONS,
  BETON_OPTIONS
};
