import { Option } from '../../Shared/MultipleSelection/type';

type TResult = {
  matchesByDate: Array<TMatchOfferInfo>;
  filteredMatches: Array<TDataTable>;
};

type TUpdateEvents = Array<{
  InfoSiteMatchId: number;
  SpreadHdp: number | undefined | null;
  SpreadOu: number | undefined | null;
  MaxbetHdp: number | undefined | null;
  NoOfHDP: string | undefined | null;
}>;

type TUpdateSpecialProduct = Array<{
  ParentId: number;
  MinorBettypeId: number;
  SpreadHdp: number | undefined | null;
  SpreadOu: number | undefined | null;
  MaxbetHdp: number | undefined | null;
  NoOfHDP: string | undefined | null;
}>;

type TMatchOfferInfo = {
  infoSiteMatchId: number;
  isNoOffer: boolean;
  eventDateTime: Required<Date>;
};
type TDataTable = TMatchOfferInfo & {
  minorBettypeId: number;
  tV: string;
  ipTv: string;
  channel: string;
  wb: string;
  time: string;
  leagueName: string;
  matchOffer: string;
  homeName: string;
  awayName: string;
  maxbetHdp: number | undefined;
  maxBetHDPNumber: number | undefined;
  noOfHDP: string | undefined;
  spreadOu: number | undefined;
  spreadHdp: number | undefined;
  isChildMatch: boolean;
  childMatchId: number;
  otherProvider: string;
  agent: string;
  venue: string;
  provider: string;
  lstNote: string;
  remark2LeagueManagement: string;
  remark: string;
  remark2: string;
  homeRemark: string;
  awayRemark: string;
  division: string;
  round: string;
  eTPK: string;
  isCheck: boolean;
  rowColor: string;
  turnoverPeriod: TTurnOverInfo[];
  marginPeriod: MarginInfo[];
  isModified: boolean | undefined;
  isReferenceLeagueManagement: boolean;
};

type TTurnOverInfo = {
  title: string;
  valueDisplay: string;
  value: number;
  color: string;
};

type MarginInfo = {
  title: string;
  valueDisplay: string;
  value: number;
  color: string;
};

type TColumnKey =
  | 'MB'
  | 'KO_Time'
  | 'League'
  | 'SpreadHDP'
  | 'SpreadOU'
  | 'NoHDP';
type TOrderKey = 'one_m' | 'three_m' | 'six_m' | 'twelve_m';
type TOrderDirection = 'none' | 'asc' | 'desc';
type TOverArrayToObject = Array<{
  infoSiteMatchId: number;
  childMatchId: number | null;
  '1m'?: number | null;
  '3m'?: number | null;
  '6m'?: number | null;
  '12m'?: number | null;
}>;
type TMultipleColumnFilterOption = {
  [key in TColumnKey]: Option[];
};

type TMultipleFilter = {
  [key in TColumnKey]: {
    isFilter: boolean;
    selected: Option[];
    options: Option[];
  };
};

type TMultipleOrder = {
  [key in TOrderKey]?: {
    isOrder: boolean;
    direction: TOrderDirection;
  };
};

type TTableSlice = {
  matchSelected: Array<TDataTable>;
  openPopupState: TOpenPopupState;
  entitiesByDate: Array<TMatchOfferInfo>;
  entities: Array<TDataTable>;
  entitiesOriginal: Array<TDataTable>;
  isNoOfferAll: boolean;
  isConfirmAll: boolean;
  isSelectedAll: boolean;
  valueFilterAllColumn: FilterAllColumn;
  columnKeyFilterCurrent: string;
  filterState: Array<TFilterState>;
  multipleFilter: TMultipleFilter;
  multipleOrder: TMultipleOrder;
  loading: string;
  previousNoOfferIndex: null | number;
};

type TOpenPopupState = {
  isOpenAddSpecialProduct: boolean;
  isOpenRemoveSpecialProduct: boolean;
};

type TFilterState = {
  columnKey: string | undefined;
  keyword: string | undefined;
};

interface FilterAllColumn {
  [x: string]: string;
}

type TPermission = {
  UserName: string;
  HasRead: boolean;
  HasWrite: boolean;
  ActorId: number;
  SiteId: number;
};

type TActionOfSpecialProduct = {
  ParentId: string;
  MinorBettypeId: string;
};

export default TDataTable;
export {
  TTurnOverInfo,
  TTableSlice,
  TFilterState,
  FilterAllColumn,
  TPermission,
  TMultipleFilter,
  TMultipleOrder,
  TColumnKey,
  TResult,
  TMultipleColumnFilterOption,
  TUpdateEvents,
  TOpenPopupState,
  TActionOfSpecialProduct,
  TUpdateSpecialProduct,
  TMatchOfferInfo,
  TOrderKey,
  TOrderDirection,
  TOverArrayToObject
};
