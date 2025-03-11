import { Option } from '../../Shared/MultipleSelection/type';

type TConfirmProvider = {
  value: number | string | null;
  label: string;
  key?: string;
  disabled?: boolean;
};

type ValidationResponse = {
  isSuccess: boolean;
  value: {
    providerVerification: TDataSource;
    sessionId: string;
  };
};

type TDataSource = {
  lstBookProvider?: string | null;
  inHouseProviderId?: string | null;
  remark?: string | null;
  remarkBy?: string | null;
  owpMatchId: number;
  parentMatchId: number;
  matchNo: string;
  matchStatus: string;
  eventDateTime: string;
  league: string;
  home: string;
  away: string;
  agentConfirm: TConfirmProvider[];
  rowColor: string | undefined;
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
  className?: string;
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
  TRemarkPopover,
  ValidationResponse
};
