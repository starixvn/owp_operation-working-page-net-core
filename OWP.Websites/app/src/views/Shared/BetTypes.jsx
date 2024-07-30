const bettypeNames = {
  1: 'Handicap',
  3: 'Over/Under',
  5: 'FT.1X2',
  7: '1H Handicap',
  8: '1H Over/Under',
  15: '1H 1X2'
};

const getBetTypeName = bettype => bettypeNames[bettype];

export { getBetTypeName };
