const section = {
  columnName: 'Section',
  columnId: 'Section'
};
const la = {
  columnName: 'LA',
  columnId: 'Username'
};
const totalmatches = {
  columnName: 'Total Matches',
  columnId: 'TotalMatches'
};

const fixedColumnLA = {
  section,
  la,
  totalmatches
};

const ORDER_ASC = 'ORDER_ASC';
const ORDER_DESC = 'ORDER_DESC';
const COLUMN_ID_HIGHLIGHT = [
  'TotalMatches',
  'TotalMultiMatches',
  'TotalSingleMatches',
  'TotalSpecialProducts',
  'TotalMixedProduct',
  'TotalESoccer'
];
const unfixedColumnLA = [
  {
    order: 1,
    name: 'MULTIPLE MATCHES',
    id: 'MULTIPLEMATCHES',
    subColumn: [
      { name: 'Link 2', id: 'Link2' },
      { name: 'Link 3', id: 'Link3' },
      { name: 'Link 4', id: 'Link4' },
      { name: 'Link 5', id: 'Link5' },
      { name: 'Link 6', id: 'Link6' },
      { name: 'Link 7', id: 'Link7' },
      { name: '>= Link 8', id: 'Link8Plus' },
      { name: 'Total Matches', id: 'TotalMultiMatches' }
    ]
  },
  {
    order: 2,
    name: 'SINGLE MATCHES',
    id: 'SINGLEMATCHES',
    subColumn: [
      { name: 'Only Provider', id: 'OnlyProvider' },
      { name: 'Only Tv', id: 'OnlyTv' },
      { name: 'Only Reporter', id: 'OnlyReporter' },
      { name: 'Mixed Provider TV Reporter', id: 'MixedProviderTVReporter' },
      { name: 'Total Matches', id: 'TotalSingleMatches' }
    ]
  },
  {
    order: 3,
    name: 'SPECIAL PRODUCTS',
    id: 'SPECIALPRODUCTS',
    subColumn: [
      { name: 'Corner+', id: 'CornerPlus' },
      { name: 'T.Booking+', id: 'TBookingPlus' },
      { name: 'Fantasy', id: 'Fantasy' },
      { name: 'Goal Kick', id: 'GoalKick' },
      { name: 'Other Products ', id: 'OtherProducts' },
      { name: 'Total Matches', id: 'TotalSpecialProducts' }
    ]
  },
  {
    order: 4,
    name: 'MIXED PRODUCTS',
    id: 'MIXED PRODUCTS',
    subColumn: [
      { name: 'FT_Corner', id: 'FTCorner' },
      { name: 'FT_Corner (5ML)', id: 'FTCorner5ML' },
      { name: 'Total Matches', id: 'TotalMixedProduct' }
    ]
  },
  {
    order: 5,
    name: '5ML',
    id: '5ML',
    subColumn: [{ name: '5ML', id: 'FiveML' }]
  },
  {
    order: 6,
    name: 'ESOCCER',
    id: 'ESOCCER',
    subColumn: [
      { name: '5 Mins', id: 'ESoccer5Mins' },
      { name: '6 Mins', id: 'ESoccer6Mins' },
      { name: '15 Mins', id: 'ESoccer15Mins' },
      { name: '16 Mins', id: 'ESoccer16Mins' },
      { name: '20 Mins', id: 'ESoccer20Mins' },
      { name: 'Total Matches', id: 'TotalESoccer' }
    ]
  },
  {
    order: 7,
    name: 'OTHERS',
    id: 'OTHERS',
    subColumn: [{ name: 'Others', id: 'Others' }]
  }
];
const PageSizes = [10, 20, 50, 100, 200, 300, 500];

const DefaultPageSize = PageSizes[0];
const TableConst = {
  unfixedColumnLA,
  fixedColumnLA,
  PageSizes,
  DefaultPageSize
};

const defaultDateModel = {
  fromDate: null,
  toDate: null
};

const millisecondsPerDay = 24 * 60 * 60 * 1000;

const dateRange = 365;

const FilterConst = {
  defaultDateModel,
  ORDER_ASC,
  ORDER_DESC,
  millisecondsPerDay,
  dateRange
};

const USER_STATUS = [
  { 'value': 'active', 'label': 'Active' },
  { 'value': 'resigned', 'label': 'Inactive' }
];

export { TableConst, FilterConst, COLUMN_ID_HIGHLIGHT, USER_STATUS };
