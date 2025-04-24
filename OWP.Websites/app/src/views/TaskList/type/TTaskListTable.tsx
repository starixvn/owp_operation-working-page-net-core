interface KOTimeProps {
  MatchNo: string;
  MatchStatus: string;
  KickOffTime: string;
  KickOffTimeDisplay: string;
  OtherTime: string | null;
  OtherTimeDisplay: string | null;
  EventDate: Date;
  EventDateDisplay: string;
}

interface PesOrFiFa {
  IsPES23: boolean;
  IsFC24: boolean;
  IsPESOther: boolean;
  IsFIFA: boolean;
  IsVietNamVLeague: boolean;
  rowIndex: number;
}

interface DataSource extends KOTimeProps, PesOrFiFa {
  OWPMatchId: Required<number>;
  ParentMatchId: Required<number>;
  Rowspan: number;
  MatchNo: string;
  MatchStatus: string;
  IsTodayEvent: boolean;
  IsSaba: boolean;
  League: Required<string>;
  LeagueGroup: string;
  WatchBet: string;
  Phone: string;
  OtherProvider: string;
  NoOfHDPId: number;
  NoOfHDP: string;
  MatchOffers: Array<{
    MatchOfferValue: string;
  }>;
  IsAllowedToViewDetail: boolean;
  Home: Required<string>;
  Away: Required<string>;
  MatchCode: string;
  AdminSiteId: number;
  IPTV: string;
  IPTVCopy: string;
  Provider: string;
  IsFantasy: boolean;
  MinorBettypeId: number;
  Remark: string;
  Round: string;
  TeamRound: string;
  Division: string;
  Channel: string;
  ReadyDateTimeDisplay: string;
  IsReady: boolean;
  IsCompletedMatch: boolean;
  Televisions: Array<{
    Color: string;
    TVCode: string;
  }>;
  EventDateTime: Date;
  Assignment: {
    AllInOne: Array<TAssignment>;
    AllInOneOthers: Array<TAssignment>;
    FastMarket: Array<TAssignment>;
    FastMarketOthers: Array<TAssignment>;
    Handicap: Array<TAssignment>;
    Handicap1st: Array<TAssignment>;
    Handicap1stOthers: Array<TAssignment>;
    HandicapOthers: Array<TAssignment>;
    LA: Array<TAssignment>;
    LAOthers: Array<TAssignment>;
    OneXTwo: Array<TAssignment>;
    OneXTwo1st: Array<TAssignment>;
    OneXTwo1stOthers: Array<TAssignment>;
    OneXTwoOthers: Array<TAssignment>;
    OverUnder: Array<TAssignment>;
    OverUnder1st: Array<TAssignment>;
    OverUnder1stOthers: Array<TAssignment>;
    OverUnderOthers: Array<TAssignment>;
    Specific10Mins: Array<TAssignment>;
    Specific10MinsOthers: Array<TAssignment>;
    Specific15Mins: Array<TAssignment>;
    Specific15MinsOthers: Array<TAssignment>;
  };
  MatchSetting: {
    FastMarket: TMatchSettingInfo;
    Handicap: TMatchSettingInfo;
    Handicap1st: TMatchSettingInfo;
    OneXTwo: TMatchSettingInfo;
    OneXTwo1st: TMatchSettingInfo;
    OverUnder: TMatchSettingInfo;
    OverUnder1st: TMatchSettingInfo;
    Specific10Mins: TMatchSettingInfo;
    Specific15Mins: TMatchSettingInfo;
  };
  HasTicket: boolean;
  IsRead: boolean;
  InfoSiteTime: string | null;
  InfoSiteTimeDisplay: string | null;
  IsShowAbsenceIcon: boolean;
}

type ModelSpecialProduct = {
  IsMajorBetType: boolean;
  BetTypeName: string;
  Maxbet: TMatchSettingInfo | null;
  IsHasMaxbetSpread: boolean;
  MainAssignment: TAssignment[];
  OthersAssignment: TAssignment[];
};

type TMatchSettingInfo = {
  BettypeId: number;
  Maxbet: number | null;
  Spread: number | null;
  MaxbetSpreadToDisplay: string;
  IsHasMaxbetSpread: boolean;
};

type TAssignment = {
  UserId: string;
  Username: string;
  Nickname: string;
  IsCurrentUser: boolean;
  IsHasContactInfo: boolean;
  Phone: string | null;
  Skype: string;
  Extension: string;
  AssignmentNote: string;
};

type TOtherAssignment = {
  assignment: TAssignment[];
};

type CellProps = {
  dataSource: Required<Readonly<Partial<DataSource>>>;
  rowIndex: Required<Readonly<number>>;
  hasDetailInformationPermission?: boolean;
  dataLength?: number;
  rowSpan?: number;
  firstMatchCheckedIsSaba?: boolean;
};

type OtherProductProps = {
  name: string;
  maxBet: TMatchSettingInfo | null;
  traders: TAssignment[];
  others: TAssignment[];
  rowIndex: number;
  rowSpan?: number;
};

type KOTimeCellProps = {
  dataSource: Required<Readonly<Partial<DataSource>>>;
  flagElement?: JSX.Element;
  eventDateElement?: JSX.Element;
  rowIndex: Readonly<number>;
};

type Column = {
  HEADER_NAME: string;
  COLUMN_KEY: string;
};

type ColumnSchema = Column & {
  width: number;
};

type TBody = {
  dataSources: Required<Readonly<Array<DataSource>>>;
  hasDetailInformationPermission: boolean;
};

type TQueryTaskList = {
  eventDateTime: Required<Readonly<Date>>;
};

type TabProps = {
  eventDate: Date;
  userId: number;
};

type TUpdateReadyStatus = {
  rowIndex: Required<number>;
  owpMatchId: Required<number>;
};

type TResponseReadyStatus = {
  readyTime: string;
  rowIndex: Readonly<number>;
};
type TResponseTable = {
  HasReadNotePermission: boolean;
  Matches: Array<DataSource>;
  HasDetailInformationPermission: boolean;
  IsHideCompletedMatches: boolean;
  ToDay: string;
  Yesterday: string;
};
type PopupState = {
  matchId: number | undefined;
  isOpen: boolean;
};
export {
  DataSource,
  Column,
  ColumnSchema,
  CellProps,
  TBody,
  TabProps,
  KOTimeCellProps,
  KOTimeProps,
  PesOrFiFa,
  TAssignment,
  TMatchSettingInfo,
  TOtherAssignment,
  TQueryTaskList,
  TResponseTable,
  TUpdateReadyStatus,
  TResponseReadyStatus,
  OtherProductProps,
  ModelSpecialProduct,
  PopupState
};
