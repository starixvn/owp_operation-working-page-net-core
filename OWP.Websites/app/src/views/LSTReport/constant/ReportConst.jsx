const section = 'Section';
const trader = 'Trader';
const match = 'Match';
const ORDER_ASC = 'sort-asc';
const ORDER_DESC = 'sort-desc';
const bgColor12 = 'bg-color12';
const bgColor11 = 'bg-color11';
const bgColor10 = 'bg-color10';
const calendar = {
  openFromDate: 'fromDate',
  openToDate: 'toDate'
};
const columnTraderPerformace = [
  {
    order: 9,
    name: 'Turnover',
    id: 'Turnover',
    subColumn: [
      { name: 'FT-HDP', color: bgColor12, id: 'FT_HDP' },
      { name: 'FT-OU', color: bgColor12, id: 'FT_OU' },
      { name: '1H-HDP', color: bgColor12, id: 'ONEH_HDP' },
      { name: '1H-OU', color: bgColor12, id: 'ONEH_OU' },
      { name: 'Other products', color: bgColor12, id: 'Other_Products' }
    ]
  },
  {
    order: 10,
    name: 'Win Loss',
    id: 'WinLoss',
    subColumn: [
      { name: 'FT-HDP', color: bgColor11, id: 'FT_HDP' },
      { name: 'FT-OU', color: bgColor11, id: 'FT_OU' },
      { name: '1H-HDP', color: bgColor11, id: 'ONEH_HDP' },
      { name: '1H-OU', color: bgColor11, id: 'ONEH_OU' },
      { name: 'Other products', color: bgColor11, id: 'Other_Products' }
    ]
  }
];

const columnTraderAssignment = [
  {
    order: 1,
    name: 'Type 1',
    id: 'Type1',
    subColumn: [
      { name: 'FT-HDP', color: 'bg-color1', id: 'FT_HDP' },
      { name: 'FT-OU', color: 'bg-color1', id: 'FT_OU' },
      { name: 'FT- HDP/OU', color: 'bg-color1', id: 'FT_HDP_OU' }
    ]
  },
  {
    order: 2,
    name: 'Type 2',
    id: 'Type2',
    subColumn: [
      { name: 'A-League', color: 'bg-color1', id: 'A_League' },
      { name: 'A-League(C)', color: 'bg-color2', id: 'A_League_C' },
      { name: 'J1', color: 'bg-color1', id: 'J1' },
      { name: 'J2', color: 'bg-color1', id: 'J2' }
    ]
  },
  {
    order: 3,
    name: 'Robot 3',
    id: 'Robot3',
    subColumn: [
      { name: 'x2', color: 'bg-color1', id: 'x2' },
      { name: 'x3', color: 'bg-color1', id: 'x3' },
      { name: 'x3+', color: 'bg-color1', id: 'x3Plus' }
    ]
  },
  {
    order: 4,
    name: 'Type 3',
    id: 'Type3',
    subColumn: [{ name: 'SOLO MJ', color: 'bg-color3', id: 'SOLO_MJ' }]
  },
  {
    order: 5,
    name: 'Type 4',
    id: 'Type4',
    subColumn: [
      { name: 'MAJOR', color: 'bg-color4', id: 'MAJOR' },
      { name: 'MIN- 1', color: 'bg-color5', id: 'MIN_1' },
      { name: 'MIN-2', color: 'bg-color6', id: 'MIN_2' },
      { name: 'MIN-3', color: 'bg-color7', id: 'MIN_3' },
      { name: 'FT-Other', color: 'bg-color13', id: 'FT_OTHER' },
      { name: '1ST(MJ)', color: 'bg-color8', id: 'ONEST_MJ' },
      { name: '1ST(other)', color: 'bg-color9', id: 'ONEST_Other' },
      { name: 'FANTASY', color: bgColor10, id: 'FANTASY' },
      { name: 'CR-NCR', color: bgColor10, id: 'CR_NCR' },
      { name: '15M', color: bgColor10, id: 'FIFTEENM' },
      { name: '10M', color: bgColor10, id: 'TENM' },
      { name: 'TB', color: bgColor10, id: 'TB' },
      { name: 'PK SO', color: bgColor10, id: 'PK_SO' },
      { name: 'Win', color: bgColor10, id: 'Win' }
    ]
  },
  {
    order: 6,
    name: 'PH-1x2',
    id: 'PH_1x2',
    subColumn: [
      { name: 'MAJOR', color: 'bg-color4', id: 'MAJOR' },
      { name: 'MIN-1', color: 'bg-color5', id: 'MIN_1' },
      { name: 'MIN-2', color: 'bg-color6', id: 'MIN_2' },
      { name: 'MIN-3', color: 'bg-color7', id: 'MIN_3' },
      { name: 'Others', color: 'bg-color7', id: 'Others' }
    ]
  },
  {
    order: 7,
    name: 'T400MINS',
    id: 'X5',
    subColumn: [{ name: 'T400MINS', color: 'bg-color1', id: 'x5' }]
  },
  {
    order: 8,
    name: 'E-SOCCERS',
    id: 'E_SOCCERS',
    subColumn: [
      { name: '5 Mins', color: 'bg-color1', id: 'ESoccer_5Mins' },
      { name: '6 Mins', color: 'bg-color1', id: 'ESoccer_6Mins' },
      { name: '15 Mins', color: 'bg-color1', id: 'ESoccer_15Mins' },
      { name: '16 Mins', color: 'bg-color1', id: 'ESoccer_16Mins' },
      { name: '20 Mins', color: 'bg-color1', id: 'ESoccer_20Mins' }
    ]
  }
];

const ReportConst = {
  calendar,
  columnTraderAssignment,
  columnTraderPerformace,
  section,
  trader,
  match,
  ORDER_ASC,
  ORDER_DESC
};

export default ReportConst;
