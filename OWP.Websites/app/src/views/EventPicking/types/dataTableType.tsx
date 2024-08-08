import { Option } from '../../Shared/MultipleSelection/type';

type TResult = {
  matchesByDate: Array<TDataTable>;
  filteredMatches: Array<TDataTable>;
};

type TUpdateEvents = Array<{
  InfoSiteMatchId: number;
  SpreadHdp: number | undefined | null;
  SpreadOu: number | undefined | null;
  MaxbetHdp: number | undefined | null;
  NoOfHDP: string | undefined | null;
}>;

type TDataTable = {
  tV: string;
  ipTv: string;
  channel: string;
  wb: string;
  eventDateTime: Required<Date>;
  leagueName: string;
  matchOffer: string;
  homeName: string;
  awayName: string;
  maxbetHdp: number | undefined;
  maxBetHDPNumber: number | undefined;
  noOfHDP: string | undefined;
  spreadOu: number | undefined;
  spreadHdp: number | undefined;
  isNoOffer: boolean;
  infoSiteMatchId: number;
  isChildMatch: boolean;
  childMatchId: number;
  otherProvider: string;
  agent: string;
  venue: string;
  provider: string;
  lstNote: string;
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

type TColumnKey = 'MB' | 'KO_Time';

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

type TTableSlice = {
  openPopupState: TOpenPopupState;
  entitiesByDate: Array<TDataTable>;
  entities: Array<TDataTable>;
  entitiesOriginal: Array<TDataTable>;
  isNoOfferAll: boolean;
  isConfirmAll: boolean;
  valueFilterAllColumn: FilterAllColumn;
  columnKeyFilterCurrent: string;
  filterState: Array<TFilterState>;
  multipleFilter: TMultipleFilter;
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

export default TDataTable;
export {
  TTurnOverInfo,
  TTableSlice,
  TFilterState,
  FilterAllColumn,
  TPermission,
  TMultipleFilter,
  TColumnKey,
  TResult,
  TMultipleColumnFilterOption,
  TUpdateEvents,
  TOpenPopupState
};
