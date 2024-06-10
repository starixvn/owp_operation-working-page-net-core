import { Option } from '../Shared/MultipleSelection/type';

type TUserDetail = {
  dataSource: Array<{
    OldUserName: string;
    NewUserName: string;
  }>;
  hasWrite: boolean;
  onToggle(value: confirmSyncUserEnum): void;
};

type TRefResource = {
  resourceId: number;
  value: string;
  name: string;
};

type TFilterOptionsResponse = {
  roles: Array<TRefResource>;
  sections: Array<TRefResource>;
  departments: Array<TRefResource>;
  categories: Array<TRefResource>;
  locationCodes: Array<TRefResource>;
  levels: Array<TRefResource>;
  languages: Array<TRefResource>;
  userGroups: Array<TRefResource>;
};

type TFilterOptions = {
  roles: Array<Option>;
  sections: Array<Option>;
  departments: Array<Option>;
  categories: Array<Option>;
  locationCodes: Array<Option>;
  levels: Array<Option>;
  languages: Array<Option>;
  userGroups: Array<Option>;
};

type TLevelOptionsResponse = {
  levelOfLiveAssistant: Array<TRefResource>;
  levelOfLiveTrading: Array<TRefResource>;
  levelOfOtherSection: Array<TRefResource>;
};

type TFilterValue = {
  userName: string;
  role: Array<Option>;
  section: Array<Option>;
  department: Array<Option>;
  category: Array<Option>;
  locationCode: Array<Option>;
};

type TFilter = {
  queryFilterOptionsStatus: string;
  filterValue: TFilterValue;
  options: TFilterOptions;
  queryFilterOptions(): Promise<void>;
  setFilterValue(value: string | Array<Option>, key: keyof TFilterValue): void;
};

type TPolicy = {
  isRead: boolean;
  isHost: boolean;
};

type TPermission = {
  queryPermissionStatus: string;
  permissions: {
    userManagement: TPolicy;
    userManagementOwnSection: TPolicy;
    userManagementAllSections: TPolicy;
  };
  queryPermissions(): Promise<void>;
};

type TSectionName = {
  querySectionNameStatus: string;
  currentSection: TRefResource | null;
  querySectionName(): Promise<void>;
};

enum confirmSyncUserEnum {
  save = 'save',
  cancel = 'cancel'
}

type TDataResponse = {
  errCode: number;
  errMsg: string | null;
} & TUpdateUserInfo;

type TUserInfo = {
  userId: number;
  userName: string;
  fullName: string;
  nickName: string;
  employeeId: string | null;
  departmentId: number | null;
  sectionId: number | null;
  sectionName: string | null;
  groupId: number | null;
  groupName: string | null;
  roleId: number | null;
  roleName: string | null;
  extension: string | null;
  phone: string | null;
  email: string | null;
  skype: string | null;
  categoryId: number | null;
  categoryName: string | null;
  locationCode: string | null;
  level: number | null;
  levelName: string | null;
  pickup: string;
  address: string | null;
  isSecondAccount: string;
  languageId: string | null;
  languages: string | null;
  flagCode: string | null
};

type TQueryUserInfo = {
  user: string | null;
  roleId: string;
  sectionId: string;
  departmentId: string;
  categoryId: string;
  locationCode: string;
};

type TDataTableResponse = Array<TUserInfo>;

type TLevelOption = {
  queryLevelOptionStatus: string;
  levelOptions: {
    levelOfLiveTrading: Array<Option>;
    levelOfLiveAssistant: Array<Option>;
    levelOfOtherSection: Array<Option>;
  };
  queryLevelOption(): Promise<void>;
  selectOption(sectionId: number | null): Array<Option>;
};

type TDataTable = {
  queryDataTableStatus: string;
  dataSource: TDataTableResponse;
  queryUserInfo(value: TQueryUserInfo): Promise<void>;
  updateValue(
    newValue: string | number,
    key: keyof TUserInfo,
    rowIndex: Required<number>
  ): void;
};

type ColumnSchema = {
  headerName: string;
  columnKey: string;
  width: number;
};
type Column = {
  headerName: string;
  columnKey: string;
};

type TUpdateUserInfo = {
  ColumnName: string;
  UserId: string;
  Value: string | null;
};

type CellProps = {
  dataSource: Array<TUserInfo>;
  rowIndex: number;
};

export {
  TUserDetail,
  confirmSyncUserEnum,
  TFilter,
  TPermission,
  TPolicy,
  TFilterOptionsResponse,
  TRefResource,
  TLevelOptionsResponse,
  TDataTable,
  TQueryUserInfo,
  TDataTableResponse,
  ColumnSchema,
  Column,
  TUserInfo,
  TLevelOption,
  CellProps,
  TUpdateUserInfo,
  TDataResponse,
  TSectionName
};
