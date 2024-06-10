export const ColumnTV = { label: 'TV/IPTV', value: 'TV/IPTV' };
export const ColumnProvider = { label: 'Provider', value: 'Provider' };

export const ColumnFullBetTypes = [ColumnTV, ColumnProvider].concat([
  { label: 'HDP', value: 'Handicap' },
  { label: 'O/U', value: 'OverUnder' },
  { label: '1H HDP', value: 'Handicap1st' },
  { label: '1H O/U', value: 'OverUnder1st' },
  { label: '1X2', value: 'OneXTwo' },
  { label: '1H 1X2', value: 'OneXTwo1st' },
  { label: '15 mins', value: 'Specific15Mins' },
  { label: '10 mins', value: 'Specific10Mins' },
  { label: 'FM', value: 'FM' },
  { label: 'AIO', value: 'AIO' }
]);

export const BettypeColumnWidth = 140;

export const ColumnPHBetTypes = [ColumnTV, ColumnProvider].concat([
  { label: '1X2', value: 'OneXTwo' },
  { label: '1H 1X2', value: 'OneXTwo1st' },
  { label: 'FM', value: 'FM' },
  { label: 'AIO', value: 'AIO' }
]);

export const BettypeId = {
  handicapSetting: 1,
  overUnderSetting: 3,
  noOfCorners: -3,
  nextCorners: -4
};

export const MatchNo = {
  columnKey: 'MatchNo',
  fixed: true,
  name: 'MatchNo',
  title: 'No#',
  width: 60
};

export const KickOffTime = {
  columnKey: 'KickOffTime',
  fixed: true,
  name: 'KickOffTime',
  title: 'KO Time',
  width: 72
};

export const Event = {
  columnKey: 'Event',
  fixed: true,
  name: 'Event',
  title: 'Event',
  width: 300
};

export const Ticket = {
  columnKey: 'Ticket',
  name: 'Ticket',
  title: 'Ticket',
  width: 50
};

export const TV = {
  columnKey: 'TV',
  fixed: false,
  name: 'TV',
  title: 'TV/IPTV',
  width: 150
};

export const Provider = {
  columnKey: 'Provider',
  fixed: false,
  name: 'Provider',
  title: 'Provider',
  width: 180
};

export const MaxbetSpread = {
  columnKey: 'MaxBet_Spread',
  fixed: false,
  name: 'MaxBet_Spread',
  title: 'MB',
  width: 115
};

export const Remark = {
  columnKey: 'Remark',
  fixed: false,
  name: 'Remark',
  title: 'Remark',
  width: 200
};

export const Note = {
  columnKey: 'Note',
  fixed: false,
  name: 'Note',
  title: 'Note',
  width: 60
};

export const LA = {
  columnKey: 'LA',
  fixed: false,
  name: 'LA',
  title: 'LA',
  width: 140
};

export const AssignmentBettype = {
  Handicap: 1,
  Handicap1st: 7,
  OverUnder: 3,
  OverUnder1st: 8,
  OneXTwo: 5,
  OneXTwo1st: 15,
  Specific15Mins: 1,
  Specific10Mins: 1,
  FM: 1,
  AIO: 1,
  LA: -1002
};

export const BetTypeSuggestion = [
  'Handicap',
  'OverUnder',
  'Handicap1st',
  'OverUnder1st'
];

export const TableInfo = {
  rowHeight: 100,
  width: 1810,
  defaultHeight: 125,
  height: 750,
  maxHeight: 750,
  headerHeight: 30
};
