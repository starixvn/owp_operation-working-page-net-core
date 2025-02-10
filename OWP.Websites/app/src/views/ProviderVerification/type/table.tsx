import { Option } from '../../Shared/MultipleSelection/type';

type TEditableKey = 'EstNote';
type TAgentConfirm = {
  Value: number | string | null;
  Label: string;
  Key?: string;
  Disabled?: boolean;
};

type TDataSource = {
  Provider?: string | null;
  OtherProvider?: string | null;
  RemarkEst?: string | null;
  RemarkBy?: string | null;
  OWPMatchId: number;
  ParentMatchId: number;
  MatchNo: string;
  MatchStatus: string;
  EventDateTime: string;
  League: string;
  Home: string;
  Away: string;
  AgentEst: string;
  AgentConfirm: TAgentConfirm[];
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

type TUpdateAgentConfirm = { value: TAgentConfirm[] } & { owpMatchId: number };

type TColumnKey =
  | 'MatchNo'
  | 'MatchStatus'
  | 'KoTime'
  | 'League'
  | 'Match'
  | 'Provider'
  | 'AgentEst'
  | 'Mb'
  | 'RemarkEst'
  | 'RemarkBy'
  | 'AgentConfirm'
  | 'EstNote'
  | 'OtherProvider';

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
  TMaxbetSetting,
  TAgentConfirm,
  TUpdateAgentConfirm,
  TMultipleColumnFilterOption,
  TMultipleFilter,
  TColumnKey,
  TMatchCell
};
