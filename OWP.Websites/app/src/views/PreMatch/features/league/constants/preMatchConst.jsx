const ORDER_ASC = 'ORDER_ASC';
const ORDER_DESC = 'ORDER_DESC';

const DefaultTableStyle = {
  rowHeight: 76,
  width: 1700,
  defaultHeight: 250,
  height: 750,
  maxHeight: 750,
  headerHeight: 38
};

const NoColumn = {
  headerName: 'No',
  columnKey: 'No',
  width: 55,
  fixed: true
};

const LeagueGroupColumn = {
  headerName: 'League Group',
  columnKey: 'LeagueGroupOrder',
  width: 120,
  fixed: true
};

const LeagueColumn = {
  headerName: 'League Name',
  columnKey: 'LeagueName',
  width: 520,
  fixed: true
};

const IssuesColumn = {
  headerName: 'EST',
  columnKey: 'Message',
  width: 50
};

const MaxbetOUColumn = {
  headerName: 'MB FT O/U',
  columnKey: 'MaxbetOU',
  width: 90
};

const MaxbetHDPColumn = {
  headerName: 'MB FT HDP',
  columnKey: 'MaxbetHDP',
  width: 90
};

const BlacklistColumn = {
  headerName: 'Black list',
  columnKey: 'Blacklist',
  width: 90
};

const HDPColumn = {
  order: 1,
  headerName: 'HDP',
  columnKey: 'HDP',
  width: 140
};

const OUColumn = {
  order: 2,
  headerName: 'O/U',
  columnKey: 'OU',
  width: 140
};

const HDPTakeOverColumn = {
  order: 3,
  headerName: 'HDP T.Over',
  columnKey: 'HDPTakeOver',
  width: 140
};

const OUTakeOverColumn = {
  order: 4,
  headerName: 'O/U T.Over',
  columnKey: 'OUTakeOver',
  width: 140
};
const NoteColumn = {
  headerName: 'Note',
  columnKey: 'Note',
  width: 55
};

const RemarkColumn = {
  headerName: 'Remark',
  columnKey: 'Remark',
  width: 180
};

const ActionColumn = {
  headerName: 'Action',
  columnKey: 'Action',
  width: 60,
  fixed: true
};

const BettypeHdpColumn = {
  bettypeId: 1,
  isTakeOver: false
};
const BettypeHdpOverColumn = {
  bettypeId: 1,
  isTakeOver: true
};

const BettypeOuColumn = {
  bettypeId: 3,
  isTakeOver: false
};

const BettypeOuOverColumn = {
  bettypeId: 3,
  isTakeOver: true
};

const ColumnModels = {
  NoColumn,
  LeagueGroupColumn,
  LeagueColumn,
  IssuesColumn,
  HDPColumn,
  OUColumn,
  HDPTakeOverColumn,
  OUTakeOverColumn,
  MaxbetOUColumn,
  MaxbetHDPColumn,
  BlacklistColumn,
  NoteColumn,
  RemarkColumn,
  ActionColumn,
  BettypeHdpColumn,
  BettypeHdpOverColumn,
  BettypeOuColumn,
  BettypeOuOverColumn
};

const TableConst = {
  ORDER_ASC,
  ORDER_DESC
};

const logAction = {
  changeStartTime: 'Change Start Time',
  updateAssignment: 'Update Assignment',
  updateOtherAssignment: 'Update Other Assignment'
};

const API_KEY = {
  VALIDATE_ASSIGNEE: 'pre-match-assignment-validate-assignment'
};

export {
  TableConst,
  NoColumn,
  LeagueGroupColumn,
  ColumnModels,
  logAction,
  DefaultTableStyle,
  API_KEY
};
