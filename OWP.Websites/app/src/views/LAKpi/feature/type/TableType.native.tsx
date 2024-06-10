type TPermission = {
  Username: string;
  HasRead: boolean;
  HasWrite: boolean;
  ActorId: number;
  SiteId: number;
};
type TSection = {
  sectionId: string;
  sectionName: string;
  isContainUser: boolean;
};
type TSectionOption = {
  label: string;
  value: string;
};

type TState = {
  dataTable: {
    loadingStatus: string;
    entities: Array<DataSource>;
    dataSourcesFiltered: Array<DataSource>;
    isGap: boolean;
  };
  filterHeaderValue: string;
  conditionData: Array<TCondition>;
  levelOption: Array<TargetOption>;
  sortState: SortState;
  permission: null | TPermission;
  sectionOption: Array<TSectionOption>;
  sectionSelected: Array<TSectionOption>;
  userStatusOption: Array<TSectionOption>;
  userStatusSelected: Array<TSectionOption>;
  loading: string;
};

type SortState = {
  columnKeySelected: string;
  orderType: string;
  propertySelected: string;
};

type TAction = {
  queryDataTable: (
    fromDate: Required<Date>,
    toDate: Required<Date>,
    section: string,
    userActive: boolean,
    userResign: boolean
  ) => Promise<void>;
  exportDataTable: (
    fromDate: Required<Date>,
    toDate: Required<Date>,
    section: string,
    userActive: boolean,
    userResign: boolean
  ) => Promise<void>;
  queryConditionData: () => Promise<void>;
  queryLevelOption: () => Promise<void>;
  updateTargetLevel: (model: Target[]) => Promise<void>;
  setDataSourcesFiltered: (data: DataSource[]) => void;
  sortDataSources: (sortState: SortState) => void;
  setFilterHeader: (value: string) => void;
  setSectionSelected: (value: Array<TSectionOption>) => void;
  setUserStatusSelected: (value: Array<TSectionOption>) => void;
  queryPermissionsAndSections: () => Promise<void>;
  getPermission: () => TPermission | null;
};

type TTotalGameInfo = {
  virtualMatches: number;
  multipleMatches: number;
  specialProducts: number;
  fiveMajorLeagues: number;
  multipleProducts: number;
};

type TLog = {
  logId: number;
  modifyBy: string;
  oldValue: string;
  newValue: string;
  modifyDate: string;
};

interface TTotalGameConditionInfo extends TTotalGameInfo {
  totalVirtualMultipleMatches: number;
}

type DataSource = {
  userId: Required<number>;
  userName: Required<string>;
  sectionName: Required<string>;
  progress: Required<string>;
  target: string | null;
  targetId: number | null;
  gapWithTarget: Required<TTotalGameInfo>;
  achievedGames: Required<TTotalGameInfo>;
  reward: number;
  isAlmostWinTarget: boolean;
};

type TCondition = {
  progress: string;
  level: string;
  conditionFirst: TTotalGameConditionInfo;
  conditionSecond: TTotalGameConditionInfo;
  timeRange: string;
  reward: string;
};

type TargetOption = {
  value: number | null;
  label: string;
};

type Target = {
  userId: number;
  targetId: number | null;
};

type SortHeaderProps = {
  columnKey: string;
  headerName: string;
  property: string;
};

export {
  TState,
  TAction,
  DataSource,
  TTotalGameInfo,
  TCondition,
  TTotalGameConditionInfo,
  SortState,
  TargetOption,
  Target,
  SortHeaderProps,
  TSection,
  TSectionOption,
  TLog
};
