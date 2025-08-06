import { buildMatchOfferURL, buildRequestURL } from './Const';

const leagueInfo = {
  id: 'new-league-details',
  label: 'New League Details'
};

const maxbetInfo = {
  id: 'league-Details',
  label: 'League Details'
};

const SpecialLiveProductInfo = {
  id: 'special-product',
  label: 'Special Live Product'
};

const maxbetSetting = {
  id: 'maxbet-setting',
  label: 'Live Settings'
};

const tvList = {
  maxLength: 20,
  inputMaxLength: 100
};

const round = {
  maxLength: 10,
  inputMaxLength: 100
};

const division = {
  maxLength: 10,
  inputMaxLength: 100
};

const status = [
  { value: 1, label: 'Active' },
  { value: 0, label: 'Inactive' }
];

const expressionOption = [
  { value: 1, label: 'Contains' },
  { value: 2, label: 'Equals' }
];

const divisionExpressionOption = [
  { value: 1, label: 'Contains' },
  { value: 2, label: 'Equals' },
  { value: 3, label: 'Pairwise Contains' }
];
const leagueCategoryOption = [
  { value: 1, label: 'Live' },
  { value: 2, label: 'Today' },
  { value: 3, label: 'Early' }
];

const numberRowsSpecialProducOption = [
  { value: 50, label: '50' },
  { value: 100, label: '100' },
  { value: 200, label: '200' },
  { value: 500, label: '500' }
];

const leagueGroupUrl = buildRequestURL('get-league-group');
const leagueGroupUnknown = { value: 0, label: 'Unknown' };
const leagueGroupAll = { value: -1, label: 'All' };
const tableMaxbetGeneralInfo = {
  rowHeight: 30,
  width: 1850,
  defaultHeight: 180,
  height: 750,
  maxHeight: 750,
  headerHeight: 30,
  groupHeaderHeight: 30
};

const viewlogUrl = buildRequestURL('viewlog');
const leagueDetailUrl = buildRequestURL('get-league-setting');
const queryConditionUrl = buildRequestURL('get-condition');
const updateConditionUrl = buildRequestURL('update-condition');
const updateTVListUrl = buildRequestURL('update-tv-list');
const getAllLeagueUrl = buildRequestURL('get-all-league');
const getMatchOffers = buildMatchOfferURL();

const maxbetGetAllLeagueAPIKey = 'league-management-get-all-league';
const leagueManagementGetLeagueSettingAPIKey =
  'league-management-get-league-setting-by-id';
const maxbetGetTVListAPIKey = 'league-management-get-tv-list';

const teamInfoAPIKey = 'league-management-get-team-info';

const maxbetUpdateLeagueIsActiveAPIKey =
  'league-management-update-league-is-active';
const maxbetUpdateLeagueIsBlacklistAPIKey =
  'league-management-update-league-is-blacklist';
const maxbetUpdateLeagueGroupAPIKey = 'league-management-update-league-group';
const leagueManagementUpdateLeagueSetting =
  'league-management-update-league-setting';
const deleteLeagueConfirmation =
  'Are you sure you want to permanently delete this category?';
const leagueManagementDeleteLeagueSettingAPIKey =
  'league-management-delete-league-setting';
const leagueManagementInsertLeagueSettingAPIKey =
  'league-management-insert-league-setting';

const noOfHDP = [
  { value: 1, label: '1HDP' },
  { value: 2, label: '2HDP' },
  { value: 3, label: '2+1H' },
  { value: 4, label: '3HDP' },
  { value: 5, label: '4HDP' },
  { value: 6, label: '2+4 lines' },
  { value: 7, label: '3+3 lines' },
  { value: 8, label: '3+4 lines' },
  { value: 9, label: '4+4 lines' },
  { value: 10, label: '1 line' },
  { value: 11, label: '2 lines' },
  { value: 12, label: '' }
];

const messageUpdateTVListSuccess = 'The TV List has been successfully updated';
const messageUpdateConditionSuccess =
  'The condition has been successfully updated';

const MaxbetConst = {
  leagueInfo,
  viewlogUrl,
  maxbetInfo,
  SpecialLiveProductInfo,
  maxbetSetting,
  leagueGroupUrl,
  status,
  leagueGroupUnknown,
  expressionOption,
  divisionExpressionOption,
  tvList,
  round,
  division,
  leagueCategoryOption,
  leagueGroupAll,
  tableMaxbetGeneralInfo,
  leagueDetailUrl,
  queryConditionUrl,
  updateTVListUrl,
  updateConditionUrl,
  getAllLeagueUrl,
  maxbetGetAllLeagueAPIKey,
  maxbetGetTVListAPIKey,
  maxbetUpdateLeagueIsActiveAPIKey,
  maxbetUpdateLeagueIsBlacklistAPIKey,
  maxbetUpdateLeagueGroupAPIKey,
  leagueManagementGetLeagueSettingAPIKey,
  leagueManagementUpdateLeagueSetting,
  noOfHDP,
  deleteLeagueConfirmation,
  leagueManagementDeleteLeagueSettingAPIKey,
  leagueManagementInsertLeagueSettingAPIKey,
  messageUpdateTVListSuccess,
  messageUpdateConditionSuccess,
  numberRowsSpecialProducOption,
  teamInfoAPIKey,
  getMatchOffers
};

export default MaxbetConst;
