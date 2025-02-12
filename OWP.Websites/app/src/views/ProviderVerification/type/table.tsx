import { Option } from '../../Shared/MultipleSelection/type';

type TEditableKey = '';
type TConfirmProvider = {
  Value: number | string | null;
  Label: string;
  Key?: string;
  Disabled?: boolean;
};

type TDataSource = {
  LSTBookProvider?: string | null;
  InhouseProviderID?: string | null;
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
  RowColor: string | undefined;
} & {
  [key in TEditableKey]: string | undefined;
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

type TEditableCell = {
  owpMatchId: number;
  columnKey: string;
  rowIndex: number;
  className?: string;
  value: string | undefined;
};

type TTicketCell = {
  [key in 'HasTicket' | 'IsRead']: boolean;
} & { rowIndex: number; dataSource: TDataSource[] };

type TUpdateRow = {
  owpMatchId: Required<number>;
  key: TEditableKey;
  value: string | undefined;
};

type TUpdateConfirmProvider = { value: TConfirmProvider[] } & { owpMatchId: number };

type TColumnKey =
  | 'MatchNo'
  | 'MatchStatus'
  | 'KoTime'
  | 'League'
  | 'Match'
  | 'LSTBookProvider'
  | 'InhouseProviderID'
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
  TEditableCell,
  TEditableKey,
  TUpdateRow,
  TConfirmProvider,
  TUpdateConfirmProvider,
  TMultipleColumnFilterOption,
  TMultipleFilter,
  TColumnKey,
  TMatchCell
};
