import {
  updateFilterKeyword,
  getKeyword,
  filterData
} from './filterHelper.native';

describe('FilterHelper', () => {
  it('Should add filter keyword success', () => {
    const filterInfo = [];
    const filterKey = 'League';
    const keyword = 'carlos';
    const expected = [
      {
        filterKey,
        keyword
      }
    ];
    expect(updateFilterKeyword(filterInfo, filterKey, keyword)).toEqual(
      expected
    );
  });

  it('Should update filter keyword success', () => {
    const filterKey = 'League';
    const filterInfo = [
      {
        filterKey,
        keyword: 'Tyler'
      }
    ];
    const keyword = 'carlos';
    const expected = [
      {
        filterKey,
        keyword
      }
    ];
    expect(updateFilterKeyword(filterInfo, filterKey, keyword)).toEqual(
      expected
    );
  });

  it('Should get keyword success', () => {
    const filterInfo = [
      {
        filterKey: 'League',
        keyword: 'Tyler'
      },
      {
        filterKey: 'LeagueGroup',
        keyword: 'M2'
      }
    ];
    const expected = 'Tyler';
    expect(getKeyword(filterInfo, 'League')).toEqual(expected);
  });

  it('Get keyword should return empty', () => {
    const filterInfo = [
      {
        filterKey: 'League',
        keyword: 'Tyler'
      },
      {
        filterKey: 'LeagueGroup',
        keyword: 'M2'
      }
    ];
    expect(getKeyword(filterInfo, 'League1')).toEqual('');
  });

  it('Should return full data if keyword empty', () => {
    const dataSource = [
      {
        GroupLabel: 'M1',
        LeagueName: 'Carlos'
      },
      {
        GroupLabel: 'M2',
        LeagueName: 'Carlos'
      }
    ];
    const filterInfo = {
      filterKey: 'Group',
      keyword: ''
    };
    const expected = dataSource;
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter group label success', () => {
    const dataSource = [
      {
        GroupLabel: 'M1'
      },
      {
        GroupLabel: 'M2'
      }
    ];
    const filterInfo = {
      filterKey: 'Group',
      keyword: 'M1'
    };
    const expected = [
      {
        GroupLabel: 'M1'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter league success', () => {
    const dataSource = [
      {
        LeagueName: 'Carlos'
      },
      {
        LeagueName: 'Tyler'
      }
    ];
    const filterInfo = {
      filterKey: 'League',
      keyword: 'T'
    };
    const expected = [
      {
        LeagueName: 'Tyler'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter min spread success', () => {
    const dataSource = [
      {
        MinSpread: '10%'
      },
      {
        MinSpread: '20%'
      }
    ];
    const filterInfo = {
      filterKey: 'Spread_Min',
      keyword: '20'
    };
    const expected = [
      {
        MinSpread: '20%'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter max spread success', () => {
    const dataSource = [
      {
        MaxSpread: '10%'
      },
      {
        MaxSpread: '20%'
      }
    ];
    const filterInfo = {
      filterKey: 'Spread_Max',
      keyword: '20'
    };
    const expected = [
      {
        MaxSpread: '20%'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter min maxbet WD success', () => {
    const dataSource = [
      {
        MinMaxbetWD: '10k'
      },
      {
        MinMaxbetWD: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'HDPWeekday_Min',
      keyword: '20k'
    };
    const expected = [
      {
        MinMaxbetWD: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter max maxbet WD success', () => {
    const dataSource = [
      {
        MaxMaxbetWD: '10k'
      },
      {
        MaxMaxbetWD: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'HDPWeekday_Max',
      keyword: '20k'
    };
    const expected = [
      {
        MaxMaxbetWD: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter min maxbet WK success', () => {
    const dataSource = [
      {
        MinMaxbetWK: '10k'
      },
      {
        MinMaxbetWK: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'HDPWeekend_Min',
      keyword: '20k'
    };
    const expected = [
      {
        MinMaxbetWK: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter max maxbet WK success', () => {
    const dataSource = [
      {
        MaxMaxbetWK: '10k'
      },
      {
        MaxMaxbetWK: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'HDPWeekend_Max',
      keyword: '20k'
    };
    const expected = [
      {
        MaxMaxbetWK: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter min FT1X2 success', () => {
    const dataSource = [
      {
        MinFT1X2: '10k'
      },
      {
        MinFT1X2: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'FT1X2_Min',
      keyword: '20k'
    };
    const expected = [
      {
        MinFT1X2: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });

  it('Should filter max FT1X2 success', () => {
    const dataSource = [
      {
        MaxFT1X2: '10k'
      },
      {
        MaxFT1X2: '20k'
      }
    ];
    const filterInfo = {
      filterKey: 'FT1X2_Max',
      keyword: '20k'
    };
    const expected = [
      {
        MaxFT1X2: '20k'
      }
    ];
    expect(filterData(dataSource, filterInfo)).toEqual(expected);
  });
});
