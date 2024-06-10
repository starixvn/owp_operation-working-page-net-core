type SummaryDetail = {
  text: Required<string>;
  style: Required<string>;
};

type CellModel = {
  dataSource: Array<DataSource>;
  columnKey: string;
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

type LogHistoryProps = {
  leagueName: string;
  handleClosePopup(): void;
};

type CellProps = {
  dataSource: Required<Array<DataSource>>;
  rowIndex: Required<number>;
};

interface DataSource {
  leagueSettingId: number;
  categoryName: Required<string>;
  osLeagueName: Required<string>;
  maxBetHDP: Required<number | null>;
  maxBetOU: Required<number | null>;
  spreadHDP: Required<number | null>;
  spreadOU: Required<number | null>;
  roundName: Required<string>;
  summaryInfo: Required<Array<SummaryDetail>>;
  summaryRemark: Required<Array<SummaryDetail>>;
  isActive: Required<boolean>;
  isActiveMaxBet: Required<boolean>;
}

export {
  DataSource,
  CellModel,
  ColumnSchema,
  Column,
  LogHistoryProps,
  CellProps
};
