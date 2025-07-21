type MatchStatus =
  | 'Live'
  | 'In-play'
  | 'Completed'
  | 'Cancel'
  | 'Postponed'
  | 'Suspended';

type Column = {
  headerName: string;
  columnKey: string;
  size: number;
  isCanFilter?: boolean;
  isCanSort?: boolean;
  sortState?: {
    columnKey: Required<string>;
    oderType: string;
  };
};

type ColumnSchema = {
  headerName: string;
  columnKey: string;
  width: number;
};

type CellModel = {
  dataSource: Array<MatchInfo>;
  columnKey: string;
  handleShowTicket(): void;
};

type CellProps = {
  dataSource: Required<Array<MatchInfo>>;
  rowIndex: Required<number>;
  firstMatchCheckedIsSaba?: boolean;
};

interface TicketCellProps extends CellProps {
  handleShowTicket(): void;
}

interface AssignmentInfo {
  AssignmentNote: string | null;
  AssignmentType: number;
  BettypeId: number;
  Extension: string | null;
  IsCancelled: boolean;
  IsCurrentUser: boolean;
  IsHasContactInfo: boolean;
  Nickname: string;
  Phone: string | null;
  Skype: string;
  UserId: number;
  Username: string;
}
interface AssignmentModel {
  AllInOne: AssignmentInfo[];
  AllInOneOthers: AssignmentInfo[];
  FastMarket: AssignmentInfo[];
  FastMarketOthers: AssignmentInfo[];
  Handicap: AssignmentInfo[];
  Handicap1st: AssignmentInfo[];
  Handicap1stOthers: AssignmentInfo[];
  HandicapOthers: AssignmentInfo[];
  LA: AssignmentInfo[];
  LAOthers: AssignmentInfo[];
  OneXTwo: AssignmentInfo[];
  OneXTwo1st: AssignmentInfo[];
  OneXTwo1stOthers: AssignmentInfo[];
  OneXTwoOthers: AssignmentInfo[];
  OverUnder: AssignmentInfo[];
  OverUnder1st: AssignmentInfo[];
  OverUnder1stOthers: AssignmentInfo[];
  OverUnderOthers: AssignmentInfo[];
  Specific10Mins: AssignmentInfo[];
  Specific10MinsOthers: AssignmentInfo[];
  Specific15Mins: AssignmentInfo[];
  Specific15MinsOthers: AssignmentInfo[];
}

interface MatchSettingInfo {
  BettypeId: number;
  Maxbet: number | null;
  Spread: number | null;
  IsHasMaxbetSpread: boolean;
  MaxbetSpreadToDisplay: string;
}

interface MatchSettingModel {
  FastMarket: MatchSettingInfo;
  Handicap: MatchSettingInfo;
  Handicap1st: MatchSettingInfo;
  OneXTwo: MatchSettingInfo;
  OneXTwo1st: MatchSettingInfo;
  OverUnder: MatchSettingInfo;
  OverUnder1st: MatchSettingInfo;
  Specific10Mins: MatchSettingInfo;
  Specific15Mins: MatchSettingInfo;
}

type ModelSpecialProduct = {
  IsMajorBetType: boolean;
  BetTypeName: string;
  IsHasMaxbetSpread: boolean;
  Maxbet: MatchSettingInfo | null;
  MainAssignment: AssignmentInfo[];
  OthersAssignment: AssignmentInfo[];
};

interface Television {
  OWPMatchId: number;
  TVId: number;
  TVCode: string;
  TVName: string;
  Decoder: string;
  Country: string;
  Color: string;
  IsSelected: boolean;
}

interface MatchOffer {
  OWPMatchId: number;
  MatchOfferValue: string;
}
interface TicketInfo {
  OwpMatchId: number;
  TicketId: number;
  LatestUsername: string;
  LatestCreatedDate: Date;
  LatestMessage: string;
  SubtypeName: string;
  IsRead: boolean;
}
interface MatchInfo {
  AdminSiteId: number;
  Assignment: AssignmentModel;
  Away: string;
  Channel: string;
  Division: string;
  EventDate: Date;
  EventDateTime: Date;
  Home: string;
  IPTV: string;
  IPTVCopy: string;
  TeamRound: string;
  IsAllowedToViewDetail: boolean;
  IsFIFA: boolean;
  IsFantasy: boolean;
  IsMinorMatch: boolean;
  IsOneHourToKickOffTime: boolean;
  IsPES23: boolean;
  IsPES24: boolean;
  IsFC24: boolean;
  IsPESOther: boolean;
  KickOffTime: string;
  League: string;
  LeagueGroup: string;
  MatchCode: string;
  MatchNo: string;
  MatchOffers: MatchOffer[];
  MatchSetting: MatchSettingModel;
  MinorBettypeId: number;
  NoOfHDP: string;
  NoOfHDPId: number;
  OWPMatchId: number;
  OtherProvider: string | null;
  OtherTime: string | null;
  OtherTimeDisplay: string | null;
  ParentMatchId: number | null;
  Phone: string;
  Provider: string | null;
  Remark: string;
  Round: string;
  Televisions: Television[];
  TicketInfos: TicketInfo[];
  WatchBet: string;
  HasTicket: boolean;
  IsRead: boolean;
  MatchStatus: MatchStatus;
  Rowspan: number;
  KickOffTimeDisplay: string;
  InfoSiteTime: string;
  InfoSiteTimeDisplay: string;
  IsShowAbsenceIcon: boolean;
  IsSaba: boolean;
}
interface FilterAllColumn {
  [x: string]: string;
}
interface FilterState {
  columnKey: string | undefined;
  keyword: string | undefined;
}
interface PopupState {
  matchId: number | undefined;
  isOpen: boolean;
}
interface RefreshModel {
  filterDate: Date;
  statusId: string;
}

interface ISortState {
  columnKey: Required<string>;
  orderType: string;
}

export {
  FilterAllColumn,
  Column,
  ColumnSchema,
  CellModel,
  CellProps,
  MatchInfo,
  TicketCellProps,
  Television,
  AssignmentInfo,
  MatchSettingInfo,
  FilterState,
  AssignmentModel,
  PopupState,
  RefreshModel,
  ISortState,
  ModelSpecialProduct
};
