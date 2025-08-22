import { Option } from '../../../../Shared/MultipleSelection/type';

interface FilterAllColumn {
  [x: string]: string;
}

type TDataSource = {
  leagueSettingId: number;
  owpLeagueId: number;
  leagueGroupId: number;
  leagueGroup: string;
  league: string;
  spread_HDP_Value: number;
  spread_OU_Value: number;
  spread_Note: string;
  spreadShow: string;
  maxbetWD_HDP_Show: string;
  wD_NoOfHDP_Show: string;
  maxbetWK_HDP_Show: string;
  wK_NoOfHDP_Show: string;
  lstNote: string;
  remark2: string;
  comment: string;
  condition: TConditionSource;
  shortLeague: string;
  blackList: boolean;
  noOfferLive: boolean;
};
type TConditionSource = {
  hasTv: boolean;
  hasIpTv: boolean;
  round: Array<TCondition>;
  division: Array<TCondition>;
  remark: string;
};

type TCondition = {
  type: 'Contains' | 'Equals' | 'Pairwise Contains';
  value: string;
};

type TTableSlice = {
  entities: Array<TDataSource>;
  entitiesOriginal: Array<TDataSource>;
  loading: string;
  leagueFilterHeader: {
    leagueName: string;
    leagueGroupId: number | null;
    BlackList:  string | (number | string | null)[];
    OfferLive:  string | (number | string | null)[];
  };
  focusSettingId: number;
  multipleFilter: TMultipleFilter;
  isOpenAddNewSetting: boolean;
  valueFilterAllColumn: FilterAllColumn;
  columnKeyFilterCurrent: string;
  filterState: Array<TFilterState>;
};

type TFilterState = {
  columnKey: string | undefined;
  keyword: string | undefined;
};

type TColumnKey =
  | 'League_Group'
  | 'League'
  | 'Tv'
  | 'Round'
  | 'League_Division'
  | 'Remark'
  | 'Spread_HDP_OU'
  | 'MB_WK_HDP'
  | 'MB_WK_NO_HDP'
  | 'MB_WD_HDP'
  | 'MB_WD_NO_HDP'
  | 'LST_Note'
  | 'Remark2'
  | 'Comment';

type TMultipleFilter = {
  [key in TColumnKey]: {
    isFilter: boolean;
    selected: Option[];
    options: Option[];
  };
};

export {
  TCondition,
  TFilterState,
  FilterAllColumn,
  TConditionSource,
  TDataSource,
  TTableSlice,
  TColumnKey,
  TMultipleFilter
};
