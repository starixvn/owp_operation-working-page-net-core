import { Option } from '../../Shared/MultipleSelection/type';

type TEditTableKey = '';
type TConfirmProvider = {
  Value: number | string | null;
  Label: string;
  Key?: string;
  Disabled?: boolean;
};

type TDataSource = {
  LstBookProvider?: string | null;
  InHouseProviderId?: string | null;
  ProviderId?: string | null;
  Provider?: string | null;
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
  | 'InHouseProviderId'
  | 'ProviderId'
  | 'Provider'
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
