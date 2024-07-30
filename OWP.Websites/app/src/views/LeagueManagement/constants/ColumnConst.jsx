const generateColumnInfo = (
  columnKey,
  headerName,
  color,
  width,
  fixed = false
) => ({
  fixed,
  columnKey,
  headerName,
  color,
  width
});

const columnWidth = 80;
const columnmatchOfferWidth = 200;
const bgColor12 = 'bg-color12';
const matchOffer = 'Match Offer';
const bgWeekend = 'bg-normal bg-weekend';
const columnLeagueDetail = {
  category: generateColumnInfo(
    'category',
    'Category',
    bgColor12,
    columnWidth,
    true
  ),
  time: generateColumnInfo('time', 'Time', bgColor12, 160, true),
  condition: {
    ...generateColumnInfo('condition', 'Condition', bgColor12, 200, true),
    subHeader: {
      hasTV: generateColumnInfo(
        'condition_hasTV',
        'Has TV',
        bgColor12,
        columnWidth,
        true
      ),
      round: generateColumnInfo(
        'condition_round',
        'Round',
        bgColor12,
        columnWidth,
        true
      ),
      division: generateColumnInfo(
        'condition_division',
        'Division',
        bgColor12,
        columnWidth,
        true
      ),
      edit: generateColumnInfo('condition_edit', 'Edit', bgColor12, 40, true)
    }
  },
  remark: generateColumnInfo('remark', 'Remark', bgColor12, 120),
  lstNote: generateColumnInfo('lstNote', 'LST Note', bgColor12, 120),
  spread: {
    ...generateColumnInfo('spread', 'Spread', 'bg-normal', 200),
    subHeader: {
      hdp: generateColumnInfo('spread_hdp', 'HDP', 'bg-normal', columnWidth),
      ou: generateColumnInfo('spread_ou', 'O/U', 'bg-normal', columnWidth),
      onehHdp: generateColumnInfo(
        'spread_onehHdp',
        '1H HDP',
        'bg-normal',
        columnWidth
      ),
      onehOu: generateColumnInfo(
        'spread_onehOu',
        '1H O/U',
        'bg-normal',
        columnWidth
      )
    }
  },
  maxbetWeekday: {
    ...generateColumnInfo(
      'maxbetWeekday',
      'Max Bet - Weekday',
      'bg-normal',
      columnWidth
    ),
    subHeader: {
      hdp: generateColumnInfo(
        'maxbetWeekday_hdp',
        'HDP',
        'bg-normal',
        columnWidth
      ),
      ou: generateColumnInfo(
        'maxbetWeekday_ou',
        'O/U',
        'bg-normal',
        columnWidth
      ),
      oneh: generateColumnInfo(
        'maxbetWeekday_oneh',
        '1H',
        'bg-normal',
        columnWidth
      ),
      noHdp: generateColumnInfo(
        'maxbetWeekday_noHdp',
        'No.HDP',
        'bg-normal',
        columnWidth
      ),
      matchOffer: generateColumnInfo(
        'maxbetWeekday_matchOffer',
        matchOffer,
        'bg-normal',
        columnmatchOfferWidth
      )
    }
  },
  maxbetWeekend: {
    ...generateColumnInfo(
      'maxbetWeekend',
      'Max Bet - Weekend',
      bgWeekend,
      columnWidth
    ),
    subHeader: {
      hdp: generateColumnInfo(
        'maxbetWeekend_hdp',
        'HDP',
        bgWeekend,
        columnWidth
      ),
      ou: generateColumnInfo('maxbetWeekend_ou', 'O/U', bgWeekend, columnWidth),
      oneh: generateColumnInfo(
        'maxbetWeekend_oneh',
        '1H',
        bgWeekend,
        columnWidth
      ),
      noHdp: generateColumnInfo(
        'maxbetWeekend_noHdp',
        'No.HDP',
        bgWeekend,
        columnWidth
      ),
      matchOffer: generateColumnInfo(
        'maxbetWeekend_matchOffer',
        matchOffer,
        bgWeekend,
        columnmatchOfferWidth
      )
    }
  },
  maxbetFT1X2: {
    ...generateColumnInfo('maxbetFT1X2', '1X2', 'bg-normal', columnWidth),
    subHeader: {
      spread: generateColumnInfo(
        'maxbetFT1X2_spread',
        'Spread',
        'bg-normal',
        columnWidth
      ),
      ft1X2: generateColumnInfo(
        'maxbetFT1X2_ft1x2',
        'FT. 1X2',
        'bg-normal',
        columnWidth
      ),
      oneh1X2: generateColumnInfo(
        'maxbetFT1X2_1h1X2',
        '1H. 1X2',
        'bg-normal',
        columnWidth
      ),
      matchOffer: generateColumnInfo(
        'maxbetFT1X2_matchOffer',
        matchOffer,
        'bg-normal',
        columnmatchOfferWidth
      )
    }
  },
  isApplied: {
    ...generateColumnInfo('isApplied', 'Is_Applied', bgColor12, columnWidth)
  },
  lastUpdated: {
    ...generateColumnInfo('lastUpdated', 'Last Updated', bgColor12, 100)
  },
  action: {
    ...generateColumnInfo('action', 'Action', bgColor12, 50)
  }
};

const generateMaxbetHeaderMinMax = (columnKey, color) => {
  return {
    min: {
      columnKey: `${columnKey}_Min`,
      headerName: 'MIN',
      color,
      width: 80
    },
    max: {
      columnKey: `${columnKey}_Max`,
      headerName: 'MAX',
      color,
      width: 80
    }
  };
};

const columnMaxbetGeneral = {
  no: {
    columnKey: 'No',
    headerName: '#',
    color: bgColor12,
    width: 50
  },
  leagueGroup: {
    columnKey: 'Group',
    headerName: 'Group',
    color: bgColor12,
    width: 80
  },
  league: {
    columnKey: 'League',
    headerName: 'League',
    color: bgColor12,
    width: 500
  },
  spread: {
    columnKey: 'spread',
    headerName: 'Spread',
    color: 'bg-normal',
    subHeader: generateMaxbetHeaderMinMax('Spread', 'bg-normal')
  },
  hDPWeekday: {
    columnKey: 'hDPWeekday',
    headerName: 'Max Bet - Weekday',
    color: 'bg-normal',
    subHeader: generateMaxbetHeaderMinMax('HDPWeekday', 'bg-normal')
  },
  hDPWeekend: {
    columnKey: 'hDPWeekend',
    headerName: 'Max Bet - Weekend',
    color: bgWeekend,
    subHeader: generateMaxbetHeaderMinMax('HDPWeekend', bgWeekend)
  },
  fT1X2: {
    columnKey: 'fT1X2',
    headerName: 'Max Bet - 1X2',
    color: 'bg-normal',
    subHeader: generateMaxbetHeaderMinMax('FT1X2', 'bg-normal')
  },
  active: {
    columnKey: 'active',
    headerName: 'Active',
    color: bgColor12,
    width: 70
  },
  blacklist: {
    columnKey: 'Blacklist',
    headerName: 'Blacklist',
    color: bgColor12,
    width: 70
  },
  lastUse: {
    columnKey: 'lastUse',
    headerName: 'Last Use',
    color: bgColor12,
    width: 150
  }
};

const columnSpecialProduct = {
  MBOfFTHDP: {
    columnKey: 'MBOfFTHDP',
    headerName: 'MB Of FT.HDP',
    color: bgColor12,
    width: columnWidth
  }
};

const ColumnConst = {
  columnLeagueDetail,
  columnMaxbetGeneral,
  columnSpecialProduct
};

export default ColumnConst;
