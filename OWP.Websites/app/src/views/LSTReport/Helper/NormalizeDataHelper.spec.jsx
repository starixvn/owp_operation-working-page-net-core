import helper from './NormalizeDataHelper';

describe('NormalizeDataHelper under test', () => {
  it('test mappingResponseToModelPerformance', () => {
    const response = [
      {
        ESoccer_5Mins: '-',
        ESoccer_6Mins: '-',
        ESoccer_15Mins: 0,
        SectionId: '16',
        SectionName: 'Casino & External'
      }
    ];
    const actual = helper.mappingResponseToModelPerformance(response);
    const expected = [
      {
        'Match': undefined,
        'Section': 'Casino & External',
        'Trader': undefined,
        'Turnover': {
          'FT_HDP': undefined,
          'FT_OU': undefined,
          'ONEH_HDP': undefined,
          'ONEH_OU': undefined,
          'Other_Products': undefined
        },
        'WinLoss': {
          'FT_HDP': undefined,
          'FT_OU': undefined,
          'ONEH_HDP': undefined,
          'ONEH_OU': undefined,
          'Other_Products': undefined
        }
      }
    ];
    expect(actual).toEqual(expected);
  });

  it('test mappingResponseToModelAssignment', () => {
    const response = [
      {
        ESoccer_5Mins: '-',
        ESoccer_6Mins: '-',
        ESoccer_15Mins: 0,
        SectionId: '16',
        SectionName: 'Casino & External'
      }
    ];
    const actual = helper.mappingResponseToModelAssignment(response);
    const expected = [
      {
        'E_SOCCERS': {
          'ESoccer_15Mins': '-',
          'ESoccer_16Mins': undefined,
          'ESoccer_20Mins': undefined,
          'ESoccer_5Mins': '-',
          'ESoccer_6Mins': '-'
        },
        'Match': undefined,
        'PH_1x2': {
          'MAJOR': undefined,
          'MIN_1': undefined,
          'MIN_2': undefined,
          'MIN_3': undefined,
          'Others': undefined
        },
        'Robot3': {
          'x2': undefined,
          'x3': undefined,
          'x3Plus': undefined
        },
        'Section': 'Casino & External',
        'Trader': undefined,
        'Type1': {
          'FT_HDP': undefined,
          'FT_HDP_OU': undefined,
          'FT_OU': undefined
        },
        'Type2': {
          'A_League': undefined,
          'A_League_C': undefined,
          'J1': undefined,
          'J2': undefined
        },
        'Type3': {
          'SOLO_MJ': undefined
        },
        'Type4': {
          'CR_NCR': undefined,
          'FANTASY': undefined,
          'FIFTEENM': undefined,
          'MAJOR': undefined,
          'MIN_1': undefined,
          'MIN_2': undefined,
          'MIN_3': undefined,
          'ONEST_MJ': undefined,
          'ONEST_Other': undefined,
          'PK_SO': undefined,
          'TB': undefined,
          'TENM': undefined,
          'Win': undefined
        },
        'X5': {
          'x5': undefined
        }
      }
    ];
    expect(actual).toEqual(expected);
  });

  it('test formatNumberWithTwoDecimal', () => {
    const actual = helper.formatNumberWithTwoDecimal(2.95738);
    expect(actual).toEqual('2.96');
  });
});
