type TColumn = {
  HEADER_NAME: string;
  COLUMN_KEY: string;
  IS_CAN_FILTER: boolean;
  WIDTH: number;
  IS_FREEZE?: boolean;
};

type TPickCellModel = {
  dataSource: TDataTable;
  columnKey: string;
  handleShowTicket: (show: any) => void; //NOSONAR
};

type TNoteCell = TCellModel & {
  onShowNote: (show: any) => void; //NOSONAR
};

type TCellModel = {
  dataSource: TDataTable;
  rowIndex: number;
};

type TAssignment = {
  AssignmentId: number | null;
  AssignmentNote: string | null;
  AssignmentType: number;
  OWPMatchId: number;
  ParentMatchId: number;
  Bettype: number;
  MinorBettypeId: number | null;
  UserId: number;
  Username: string;
};

type TBettype =
  | 'Handicap'
  | 'Handicap1st'
  | 'OverUnder'
  | 'OverUnder1st'
  | 'OneXTwo'
  | 'OneXTwo1st'
  | 'Specific15Mins'
  | 'Specific10Mins'
  | 'FM'
  | 'AIO'
  | 'LA';

type TDataSource = {
  Channel: boolean;
  Phone: string;
  WatchBet: string;
  OtherProvider: string;
  Round: string;
  Division: string;
  TeamRound: string;
  Remark: string;
  IsFantasy: boolean;
  IsSaBa: boolean;
  MinorBettypeId: number;
  MatchCode: string;
  NoOfHDPId: number | null;
  AdminSiteId: number | null;
  IPTVCopy: string;
  Provider: string;
  OWPMatchId: number;
  ParentMatchId: number;
  MatchStatus: string;
  InfoSiteMatchId: null | number;
  IsInfoSiteLinked: boolean;
  IsLiveStatusSynchronized: boolean;
  IsShowAbsenceIcon: boolean;
  MatchNo: string | null;
  KickOffTime: string;
  KickOffTimeStr: string;
  InfoSiteTime: string | null;
  InfoSiteTimeDisplay: string | null;
  OtherTime: string;
  OtherTimeStr: string;
  Home: string;
  Away: string;
  League: string;
  LeagueGroup: string | null;
  IsRead: boolean | null;
  HasTicket: boolean;
  NoOfHDP: string | null;
  NoOfHDPShort: string | null;
  AIOName: string | null;
  AIOShortName: string | null;
  SOP: string | null;
  IsAllowedToViewDetail: boolean;
  MatchOffers: Array<{
    Id: number;
    OWPMatchId: number;
    MatchOfferValue: string;
    MatchOfferShortName: string;
  }>;
  Notes: Array<TNote>;
  Handicap: Array<TAssignment>;
  Handicap1st: Array<TAssignment>;
  OverUnder: Array<TAssignment>;
  OverUnder1st: Array<TAssignment>;
  OneXTwo: Array<TAssignment>;
  OneXTwo1st: Array<TAssignment>;
  Specific15Mins: Array<TAssignment>;
  Specific10Mins: Array<TAssignment>;
  FM: Array<TAssignment>;
  AIO: Array<TAssignment>;
  LA: Array<TAssignment>;
  HandicapSetting: TMaxbetSpread;
  OverUnderSetting: TMaxbetSpread;
  Televisions: Array<{
    TVCode: string;
    Color: string;
  }>;
  Website1: string | null | undefined;
  Website2: string | null | undefined;
  Website3: string | null | undefined;
  Website4: string | null | undefined;
};
type TColumnFilterByMbc = {
  maxbetHDP: number | string;
  spreadHDP: number | string;
  spreadOU: number | string;
  maxBetToDisplay: string;
  keyword: string;
};
type TMaxbetSpread = {
  Maxbet: number;
  Spread: number;
  MaxBetToDisplay: string;
  SpreadToDisplay: string;
};
type TCellSchema = {
  columnKey: string;
  dataSource: TDataTable;
  rowIndex: number;
  onShowPopup: (show: boolean) => void;
};

type TDataTable = Array<TDataSource>;
type TQueryDataResult = {
  data: TDataTable;
  dateUpdated: string;
  syncOffVRLastUpdated: {
    item1: string;
    item2: string;
  };
};
type TNote = {
  LastUpdated: string;
  MatchNoteId: number;
  Note: string;
  OWPMatchId: number;
  UserId: number;
  Username: string;
};
type TAllowAddEvent = {
  isYesterdayOrAfter: boolean;
  isAllowAddEvent: boolean;
};

type TQueryLiveGameAssignment = {
  dateUpdated: string;
  syncOffVRLastTime: string;
  syncOffVRByUser: string;
  filterState: Array<TColumnFilter>;
  loading: string;
  entities: TDataTable;
  matchesSelected: TDataTable,
  entitiesOriginal: TDataTable;
  isShowUnreadMsg: boolean;
};

type TPolicy = {
  UserId: number;
  Username: string;
  FullBetTypes: {
    HasWrite: boolean;
  };
  PHBetTypes: {
    HasWrite: boolean;
  };
  detailInformationPolicy: {
    HasRead: boolean;
    HasWrite: boolean;
  };
  EventPicking: {
    HasRead: boolean;
    HasWrite: boolean;
  };
  dailyWorkingShiftPolicy: {
    HasRead: boolean;
    HasWrite: boolean;
  };
  MyTicket: {
    HasRead: boolean;
    HasWrite: boolean;
  };
  Note: {
    HasRead: boolean;
    HasWrite: boolean;
  };
  Odds: {
    HasRead: boolean;
    HasWrite: boolean;
  };
};

type TAssignmentBuilder = {
  dataSource: TDataTable;
  rowIndex: number;
  bettypeName: TBettype;
};

type TUpdateAssignment = {
  assignment: TAssignment;
  bettypeName: TBettype;
  rowIndex: number;
};

type TColumnFilter = {
  keyword: string;
  columnKey: string;
};

type TFilterQueryOptions = {
  MatchStatus: TMatchStatus;
  OffVR: TMatchOffer;
};

type TMatchStatus = {
  RefName: string;
  Value: string;
};

type TMatchOffer = {
  Id: number;
  RefName: string;
  Value: string;
};

type TQueryPolicy = {
  loading: string;
  entities: TPolicy;
};

type TFilter = {
  selectedDate: Date;
  userIds: string;
  userNames: string;
  selectedMatchType: TOptions[];
  selectedMatchStatus: TOptions[];
  selectedSyncLive: TOptions;
  selectedOffVRs: TOptions[];
};

type TOptions = {
  label: string;
  value: string | number;
};

type TTicketCell = TCellModel & {
  onShowTicketList: (show: any) => void;  //NOSONAR
};
type CreatorInfo = {
  groupName: null | string;
  nickName: null | string;
  recipientId: number;
  recipientType: number;
  userName: string;
};

type TQueryDataModel = {
  EventDate: string;
  UserIds: string;
  MatchStatusIds: Array<number | string>;
  SelectedOffVRs: Array<number | string>;
  MatchType: string;
  SyncLiveStatus: number | string;
};

type TEventDetail = {
  OWPMatchId: number;
};

export {
  TQueryDataModel,
  TQueryDataResult,
  TDataSource,
  TOptions,
  TFilterQueryOptions,
  TMatchOffer,
  TDataTable,
  TQueryLiveGameAssignment,
  TColumn,
  TPickCellModel,
  TCellModel,
  TQueryPolicy,
  TFilter,
  TPolicy,
  TTicketCell,
  CreatorInfo,
  TAssignmentBuilder,
  TBettype,
  TNoteCell,
  TNote,
  TAssignment,
  TUpdateAssignment,
  TColumnFilter,
  TColumnFilterByMbc,
  TMaxbetSpread,
  TEventDetail,
  TAllowAddEvent,
  TCellSchema
};
