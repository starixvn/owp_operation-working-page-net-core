interface IObjectKeys {
  [key: string]: number | string;
}

type LogValue = {
  modifyBy: string;
  modifyDate: string | null;
  keyText: string;
  valueNew: string | null;
  valueOld: string | null;
};

interface MaxBetSettingModel extends IObjectKeys {
  fT_HDP: number;
  fT_OU: number;
  firstHalf_HDP: number;
  firstHalf_OU: number;
  noOfCorner_FT_HDP: number;
  noOfCorner_FT_OU: number;
  noOfCorner_1H_HDP: number;
  noOfCorner_1H_OU: number;
  nextCorner_FT_HDP: number;
  totalBooking_FT_HDP: number;
  totalBooking_FT_OU: number;
  totalBooking_1H_HDP: number;
  totalBooking_1H_OU: number;
  nextBooking_FT_HDP: number;
  secondAccept_Main: string;
  secondAccept_Corner: string;
  secondAccept_TotalBooking: string;
  secondAccept_Fantasy: string;
  lastUpdatedBy: string;
}
interface LogData {
  action: number;
  oldValue: MaxBetSettingModel;
  newValue: MaxBetSettingModel;
  modifyDate: string;
  modifyBy: string;
}

class EditPopupModel {
  value: number | null = 0;
  fTHDP = 0;
  column = '';
}

interface EditModel {
  fTHDP: number;
  column: string;
  oldValue: number | string;
}

interface SelectOption {
  value: number;
  label: string;
}

type Policy = {
  Username: string | null;
  HasRead: boolean | null;
  HasWrite: boolean | null;
  ActorId: number | null;
  SiteId: number | null;
};

type SubColumnModel = {
  name: string;
  id: string;
};

type ColumnModel = {
  name: string;
  id: string;
  subColWidth: number;
  fixed: string;
  subColumn: SubColumnModel[];
};

type SecondAccept = {
  Id: number;
  Value: string;
};

type MaxbetSettingState = {
  isOpenLogPopup: boolean;
  maxBetValue: number;
  logData: LogData[];
  loading: string;
  editModel: EditModel;
  isOpenEditPopup: boolean;
  popupType: number | null;
  policy: Policy;
  maxbetSettings: MaxBetSettingModel[];
  secondAcceptValues: SelectOption[];
  getMaxbetSettings: () => Promise<void>;
  queryPolicy: () => Promise<void>;
  setOpenEditPopup: () => void;
  setCloseEditPopup: () => void;
  setPopupType: (type: number) => void;
  setEditModel: (value: EditModel) => void;
  updateMaxbetSetting: (object: EditPopupModel) => Promise<void>;
  getSecondAcceptValues: () => Promise<void>;
  setLogData: (maxBetValue: number) => Promise<void>;
  setOpenLogPopup: () => void;
  setCloseLogPopup: () => void;
};

export {
  MaxBetSettingModel,
  EditPopupModel,
  EditModel,
  SelectOption,
  Policy,
  LogValue,
  ColumnModel,
  SubColumnModel,
  SecondAccept,
  MaxbetSettingState,
  LogData
};
