import { ParamFilter } from './filterType';
import { ISettingDetail } from './settingDetailType';
import { DataSource } from './tableType';
import { PopupType, TCommonType } from './type';

type LogValue = {
  title: 'Created the setting' | 'Changed the setting';
  actionId: number;
  modifyBy: string;
  modifyDate: string | null;
  keyText: string;
  valueNew: string | null;
  valueOld: string | null;
};

type TPolicy = {
  Username: string | null;
  HasRead: boolean | null;
  HasWrite: boolean | null;
  ActorId: number | null;
  SiteId: number | null;
};

interface LogData {
  [key: string]: string | null;
}

type TErrorMessage = {
  leagueName: null | string;
  offerLines: null | string;
  betDelay: null | string;
  maxbetHDP: null | string;
  maxbetOU: null | string;
  betTypes: null | string;
  round: null | string;
  provider: null | string;
  rangeTime: null | string;
  iptv: null | string;
  remark: null | string;
  valid: boolean;
};

type LogQuery = {
  action: number;
  modifyDate: string;
  modifyBy: string;
  oldValue: LogData;
  newValue: LogData;
};

interface OsLeagueState {
  filter: {
    [x: string]: string;
  };
  policy: TPolicy;
  settingInfoOrigin: ISettingDetail;
  settingInfo: ISettingDetail;
  settingInfoDefault: ISettingDetail;
  entities: Array<DataSource>;
  logData: Array<LogQuery>;
  loading: string;
  errorMessage: TErrorMessage;
  errorMessageDefault: TErrorMessage;
  getLogSettingQuery(leagueSettingId: number): Promise<void>;
  setFilter: (props: ParamFilter) => void;
  popupType: PopupType;
  queryDataSource(): Promise<void>;
  queryLeagueDetail: (value: number) => void;
  setPopupType: (value: PopupType) => void;
  saveSetting: (segment: string) => Promise<number>;
  validateBeforeSubmit: () => boolean;
  setPolicy: (value: TPolicy) => void;
  getPolicy: () => TPolicy;
  isChangeData: (boolean: boolean) => boolean;
  setSettingInfo: (
    key: string,
    value: Array<string | number> | TCommonType
  ) => void;
  resetSetting: () => void;
  resetErrorMessage: () => void;
}

export { TPolicy, OsLeagueState, LogQuery, LogValue, TErrorMessage };
