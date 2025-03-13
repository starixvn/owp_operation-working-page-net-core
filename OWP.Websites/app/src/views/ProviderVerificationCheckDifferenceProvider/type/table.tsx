import { Option } from '../../Shared/MultipleSelection/type';

type TEditTableKey = '';
type TConfirmProvider = {
  Value: number | string | null;
  Label: string;
  Key?: string;
  Disabled?: boolean;
};

type TDataSource = {
  lstBookProvider?: string | null;
  inHouseProviderId?: string | null;
  providerMatchId?: string | null;
  provider?: string | null;
  remark?: string | null;
  remarkBy?: string | null;
  owpMatchId?: number;
  parentMatchId: number;
  matchNo: string;
  matchStatus: string;
  eventDateTime?: string;
  league: string;
  home: string;
  away: string;
  agentConfirm: TConfirmProvider[];
  rowColor: string | undefined;
} & {
  [key in TEditTableKey]: string | undefined;
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

type TEditTableCell = {
  owpMatchId: number;
  columnKey: string;
  rowIndex: number;
  className?: string;
  value: string | undefined;
};

type TTicketCell = {
  [key in 'HasTicket' | 'IsRead']: boolean;
} & { rowIndex: number; dataSource: TDataSource[]; };

type TUpdateRow = {
  owpMatchId: Required<number>;
  key: TEditTableKey;
  value: string | undefined;
};

type TUpdateConfirmProvider = { value: TConfirmProvider[]; } & { owpMatchId: number; };

type TColumnKey =
  | 'MatchNo'
  | 'KoTime'
  | 'League'
  | 'Match'
  | 'LstBookProvider'
  | 'Provider'
  | 'ConfirmProvider';

type TMultipleColumnFilterOption = {
  [key in TColumnKey]: Option[];
};

type TMultipleFilter = {
  [key in TColumnKey]: {
    isFilter: boolean;
    selected: Option[];
  };
};

export {
  TDataSource,
  TCommonCell,
  TTicketCell,
  TEditTableCell,
  TEditTableKey,
  TUpdateRow,
  TConfirmProvider,
  TUpdateConfirmProvider,
  TMultipleColumnFilterOption,
  TMultipleFilter,
  TColumnKey,
  TMatchCell
};
