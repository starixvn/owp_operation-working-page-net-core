import {
  AssignmentModel,
  FilterState,
  MatchInfo,
  ModelSpecialProduct
} from '../type/TableType';
import { MatchStatuses } from '../../Shared/Constants';
import { filterDataTable } from '../../Shared/Utils';

const buildColorRowTable = (dataSource: MatchInfo) => {
  if (!dataSource) {
    return '';
  }
  switch (dataSource.MatchStatus) {
    case MatchStatuses.Cancel:
    case MatchStatuses.Postponed:
      return 'orange-row';
    case MatchStatuses.Suspended:
      return 'red-row';
    default:
      return '';
  }
};

const searchCommonInfo = (
  source: MatchInfo[],
  keys: Array<keyof MatchInfo>,
  keyword: string
) =>
  source.filter(x =>
    keys.some(i => x[i]?.toString().toLowerCase().includes(keyword.trim()))
  );

const searchTraderInfo = (
  source: MatchInfo[],
  nestedKeys: Array<keyof AssignmentModel>,
  keyword: string
) =>
  source.filter(({ Assignment }) =>
    isAssignment(Assignment, nestedKeys, keyword)
  );

const isAssignment = (
  assignment: AssignmentModel,
  assignmentKeys: Array<keyof AssignmentModel>,
  keyword: string
) =>
  assignmentKeys.some(i =>
    assignment[i].some(
      ({ Nickname, Username }) =>
        Nickname?.toLowerCase().includes(keyword.trim()) ||
        Username?.toLowerCase().includes(keyword.trim())
    )
  );

const commonKeys: { [key: string]: Array<keyof MatchInfo> } = {
  ['No #']: ['MatchNo'],
  ['League']: ['League'],
  ['Match']: ['Home', 'Away'],
  ['Provider']: ['WatchBet', 'Phone', 'Provider', 'OtherProvider'],
  ['TV/IPTV']: ['IPTVCopy']
};

const traderKeys: { [key: string]: Array<keyof AssignmentModel> } = {
  ['FT Trader']: ['Handicap', 'HandicapOthers', 'OverUnder', 'OverUnderOthers'],
  ['1H Trader']: [
    'Handicap1st',
    'Handicap1stOthers',
    'OverUnder1st',
    'OverUnder1stOthers'
  ],
  ['Other Product Trader']: [
    'Specific10MinsOthers',
    'Specific15MinsOthers',
    'Specific10Mins',
    'Specific15Mins',
    'AllInOne',
    'AllInOneOthers',
    'FastMarket',
    'FastMarketOthers',
    'OneXTwo',
    'OneXTwoOthers',
    'OneXTwo1st',
    'OneXTwo1stOthers'
  ],
  ['LA']: ['LA', 'LAOthers']
};

const searchLeagueInfo = (
  source: MatchInfo[],
  leagueKeys: Array<keyof MatchInfo>,
  keyword: string
) => source.filter(x => x.League?.toLowerCase().includes(keyword.trim()));

const filterDataByKeys = (
  source: MatchInfo[],
  columnKey: string,
  keyword: string
) => {
  // if (columnKey === 'League') {
  //   return searchLeagueInfo(source, ['League'], keyword);
  // }
  if (columnKey in commonKeys) {
    return searchCommonInfo(source, commonKeys[columnKey], keyword);
  }
  if (columnKey in traderKeys) {
    return searchTraderInfo(source, traderKeys[columnKey], keyword);
  }
  
  return [];
};

const filterDataTableByCriteria = (
  data: MatchInfo[],
  { columnKey, keyword }: FilterState
) =>
  columnKey && keyword
    ? filterDataByKeys(data, columnKey, keyword.toLowerCase())
    : data;

const filterIssueByUnreadMessageValue = (
  entities: MatchInfo[],
  isShowUnreadMessOnly: boolean
) => entities.filter(d => !d.IsRead || !isShowUnreadMessOnly);

const filterData =
  (entities: MatchInfo[]) => (isShowUnreadMessOnly: boolean) => {
    const data = filterIssueByUnreadMessageValue(
      entities,
      isShowUnreadMessOnly
    );
    return (filters: FilterState[]) =>
      filterDataTable(data, filters, filterDataTableByCriteria);
  };

const sortDataAsc = (entities: MatchInfo[]) =>
  [...entities].sort(
    (src, dest) =>
      new Date(src.EventDateTime).getTime() -
        new Date(dest.EventDateTime).getTime() ||
      src.MatchNo.localeCompare(dest.MatchNo)
  );

const sortDataDesc = (entities: MatchInfo[]) =>
  [...entities].sort(
    (src, dest) =>
      new Date(dest.EventDateTime).getTime() -
        new Date(src.EventDateTime).getTime() ||
      dest.MatchNo.localeCompare(src.MatchNo)
  );

const buildSpecialListModel = (dataSource: MatchInfo) => {
  const specialListModel: Array<ModelSpecialProduct> = [
    {
      IsMajorBetType: true,
      BetTypeName: 'FM',
      Maxbet: dataSource.MatchSetting?.FastMarket,
      IsHasMaxbetSpread: dataSource.MatchSetting?.FastMarket.IsHasMaxbetSpread,
      MainAssignment: dataSource.Assignment?.FastMarket,
      OthersAssignment: dataSource.Assignment?.FastMarketOthers
    },
    {
      IsMajorBetType: true,
      BetTypeName: 'AIO',
      Maxbet: null,
      IsHasMaxbetSpread: false,
      MainAssignment: dataSource.Assignment?.AllInOne,
      OthersAssignment: dataSource.Assignment?.AllInOneOthers
    },
    {
      IsMajorBetType: false,
      BetTypeName: '15mins',
      IsHasMaxbetSpread:
        dataSource.MatchSetting?.Specific15Mins.IsHasMaxbetSpread,
      Maxbet: dataSource.MatchSetting?.Specific15Mins,
      MainAssignment: dataSource.Assignment?.Specific15Mins,
      OthersAssignment: dataSource.Assignment?.Specific15MinsOthers
    },
    {
      IsMajorBetType: false,
      BetTypeName: '10mins',
      Maxbet: dataSource.MatchSetting?.Specific10Mins,
      IsHasMaxbetSpread:
        dataSource.MatchSetting?.Specific10Mins.IsHasMaxbetSpread,
      MainAssignment: dataSource.Assignment?.Specific10Mins,
      OthersAssignment: dataSource.Assignment?.Specific10MinsOthers
    }
  ];
  return specialListModel.filter(
    x => x.MainAssignment?.length > 0 || x.IsHasMaxbetSpread
  );
};

export {
  filterData,
  buildColorRowTable,
  sortDataAsc,
  sortDataDesc,
  buildSpecialListModel
};
