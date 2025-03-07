import { Option } from '../../Shared/MultipleSelection/type';

type TConfirmProvider = {
  Value: number | string | null;
  Label: string;
  Key?: string;
  Disabled?: boolean;
};

type TDataSource = {
  LstBookProvider?: string | null;
  InHouseProviderId?: string | null;
  Remark?: string | null;
  RemarkBy?: string | null;
  OWPMatchId: number;
  ParentMatchId: number;
  MatchNo: string;
  MatchStatus: string;
  EventDateTime: string;
  League: string;
  Home: string;
  Away: string;
  ConfirmProvider: TConfirmProvider[];
  AgentConfirm: TConfirmProvider[];
  RowColor: string | undefined;
} & TTicketCell;

type TCommonCell = {
  className?: string;
  value: string | null | undefined;
  index: number;
  isCanToolTip: boolean;
};

type TMatchCell = {
  className: string;
  home: string;
  away: string;
};

type TRemarkCell = {
  className?: string;
  OWPMatchId: number;
  value: string | null | undefined;
  index: number;
  isRemark: boolean;
};

type TRemarkPopover = {
  hasWrite: boolean;
  OWPMatchId: number;
  value: string | null | undefined;
  index: number;
  isRemark: boolean;
};

type TTicketCell = {
  [key in 'HasTicket' | 'IsRead']: boolean;
} & { rowIndex: number; dataSource: TDataSource[]; };

type TUpdateConfirmProvider = { value: TConfirmProvider[]; } & { owpMatchId: number; };

type TColumnKey =
  | 'MatchNo'
  | 'MatchStatus'
  | 'KoTime'
  | 'League'
  | 'Match'
  | 'LstBookProvider'
  | 'InHouseProviderId'
  | 'ConfirmProvider'
  | 'Remark'
  | 'RemarkBy';

type TMultipleColumnFilterOption = {
  [key in TColumnKey]: Option[];
};

type TMultipleFilter = {
  [key in TColumnKey]: {
    isFilter: boolean;
    selected: Option[];
  };
};

type TRemark = {
  Remark: string;
  owpMatchId: number;
};

export {
  TDataSource,
  TCommonCell,
  TTicketCell,
  TRemarkCell,
  TConfirmProvider,
  TUpdateConfirmProvider,
  TMultipleColumnFilterOption,
  TMultipleFilter,
  TColumnKey,
  TMatchCell,
  TRemark,
  TRemarkPopover
};
