export const MatchStatuses = {
  Live: 'Live',
  InPlay: 'In-play',
  Completed: 'Completed',
  Cancel: 'Cancel',
  Suspended: 'Suspended',
  Postponed: 'Postponed'
};

export const AssignmentTypes = {
  Main: 1,
  Other: 2
};

export const MajorBetTypes = {
  Handicap: 1,
  OverUnder: 3,
  OneXTwo: 5,
  Handicap1H: 7,
  OverUnder1H: 8,
  OneXTwo1H: 15,
  LA: -1002
};
export const Choice = {
  One: 1,
  Two: 2,
  Three: 3,
  None: null
};
export const MinorBetTypes = {
  Specific15mins: -1,
  Specific10mins: -2,
  NoOfCorners: -3,
  NextCorners: -4,
  Specific15minsNoOfCorners: -5,
  TotalBooking: -6,
  NextBooking: -7,
  Specific15minsTotalBooking: -8,
  FastMarket: -88,
  AllInOne: -89,
  AllInOneTotalBookings: -189,
  AllInOneCorners: -289,
  FastMarketTotalBookings: -188,
  FastMarketCorners: -288
};

export const allBettype = [
  { value: 1, label: 'HDP' },
  { value: 3, label: 'O/U' },
  { value: 5, label: 'FT.1X2' },
  { value: 7, label: '1H HDP' },
  { value: 8, label: '1H O/U' },
  { value: 15, label: '1H 1X2' },
  { value: -1, label: 'XMins' },
  { value: -88, label: 'FM' }
];

export const pageSizeOptions = [
  { value: 10, label: '10' },
  { value: 20, label: '20' },
  { value: 30, label: '30' },
  { value: 50, label: '50' },
  { value: 100, label: '100' },
  { value: 200, label: '200' },
  { value: 500, label: '500' },
  { value: 1000, label: '1000' }
];

export const bettypeIdCommon = [1, 3, 5, 7, 8, 15];
export const bettypeIdNoOfCornersTotalBooking = [1, 3, 5, 7, 8, 15, -1];
export const bettypeIdMainMatch = [1, 3, 5, 7, 8, 15, -1, -88];
export const bettypeIdDetectNoOfCornersTotalBooking = [-3, -6];
export const bettypeIdXminOfNoOfCornersTotalBooking = [-5, -8];
export const bettypeIdXmins = -1;
export const PageSizes = [10, 20, 50, 100, 200, 300, 500];
export const DefaultPageSize = PageSizes[4];
export const TimePickerDisplayFormat = 'h:mm A';
export const TimePickerOutputFormat = 'HH:mm';

export const NET_CORE_QUERY_SIMPLE_API = `${baseApiUrl}/state-api/simple-data/query`;
export const NET_CORE_UPDATE_SIMPLE_API = `${baseApiUrl}/state-api/simple-data/update`;
export const buildQuerySimpleApi = () => `${NET_CORE_QUERY_SIMPLE_API}`;
export const buildUpdateSimpleApi = () => `${NET_CORE_UPDATE_SIMPLE_API}`;

export const NET_CORE_FILE_UPLOAD = `${baseApiUrl}/api/FileUpload`;

export const FunctionKeys = {
  LiveGameAssignment_PHBetTypes: 'LiveGameAssignment_PHBetTypes',
  LiveGameAssignment_FullBetTypes: 'LiveGameAssignment_FullBetTypes',
  DailyWorkingShift: 'DailyWorkingShift',
  OddsCompiler: 'OddsCompiler',
  Note: 'Note',
  DetailsInformation: 'DetailsInformation',
  QuickSearch: 'QuickSearch',
  NotificationSetting_MySectionMessage: 'NotificationSetting_MySectionMessage',
  UserManagement: 'UserManagement',
  TraderReport_TraderAssignment: 'TraderReport_TraderAssignment',
  TraderReport_TraderPerformance: 'TraderReport_TraderPerformance',
  MaxBetManagement: 'MaxBetManagement',
  LSTTeam: 'LSTTeam',
  LeagueManagement_EditConditions: 'LeagueManagement_EditConditions',
  LeagueManagement_General: 'LeagueManagement_General',
  LeagueManagement_LSTNote: 'LeagueManagement_LSTNote',
  LeagueManagement_TVList: 'LeagueManagement_TVList',
  EventPicking: 'EventPicking',
  EventPickingNew: 'EventPickingNew',
  EventPickingNew_EventAndPerformance: 'EventPickingNew_EventAndPerformance',
  La_Report: 'LA_Report',
  DayShiftAssignment: 'DayShiftAssignment',
  NightShiftAssignment: 'NightShiftAssignment',
  Tickets: 'Tickets',
  TicketCenter: 'MyTickets',
  CreateTickets: 'CreateTickets',
  DeleteTickets: 'DeleteTickets',
  OSLiveGameAssignment: 'OSLiveGameAssignment',
  AutoAssignment: 'AutoAssignment',
  LiveGameList: 'LiveGameList',
  TimeRangeView: 'TimeRangeView',
  FullTimeView: 'FullTimeView',
  ProviderArrangement: 'ProviderArrangement',
  ProviderVerification: 'ProviderVerification'
};

export const REQUEST_STATUS = {
  idle: 'idle',
  pending: 'pending'
};

export const EventNames = {
  CloseDetailModal: 'close-detail-modal'
};

export const ORDERBY = {
  ASC: 'asc',
  DESC: 'desc'
};

export const BettypeId = {
  handicapSetting: 1,
  overUnderSetting: 3,
  noOfCorners: -3,
  nextCorners: -4
};

export const baseApiUrl =
  process.env.NODE_ENV === 'development'
    ? `${location.origin}`
    : `${location.origin}/n1`;
