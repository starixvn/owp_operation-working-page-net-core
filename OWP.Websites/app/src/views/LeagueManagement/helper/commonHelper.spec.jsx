import {
  rebuildLeagueGroupOption,
  excludeOldLeague,
  updateLeagueGroup,
  updateActive,
  updateBlackList,
  calculateNumberOfDaysBetweenTwoDates,
  buildModelTVList,
  buildModelCondition,
  hasWriteOnPageMaxbetGeneral,
  updateLeagueDetail,
  removeLeagueDetail
} from './commonHelper';

describe('commonHelper', () => {
  it('Should rebuild league group option success', () => {
    const option = [
      {
        value: 1,
        label: 'M1'
      },
      {
        value: 2,
        label: 'M2'
      }
    ];

    const expected = option.concat([{ value: 0, label: '' }]);
    expect(rebuildLeagueGroupOption(option)).toEqual(expected);
  });

  it('Should exclude old league success', () => {
    const dataSource = [
      {
        LastUse: '03-11-2020'
      },
      {
        LastUse: '03-11-2018'
      }
    ];
    const expected = [];
    expect(excludeOldLeague(dataSource)).toEqual(expected);
  });

  it('Should update league group success', () => {
    const dataSource = [
      {
        GroupValue: 1,
        GroupLabel: 'M1'
      },
      {
        GroupValue: 2,
        GroupLabel: 'Mj'
      }
    ];
    const model = {
      rowIndex: 0,
      GroupLabel: 'M3',
      GroupValue: 3
    };
    const expected = [
      {
        GroupValue: 3,
        GroupLabel: 'M3'
      },
      {
        GroupValue: 2,
        GroupLabel: 'Mj'
      }
    ];
    expect(updateLeagueGroup(dataSource, model)).toEqual(expected);
  });

  it('Should update active success', () => {
    const dataSource = [
      {
        IsActive: true
      },
      {
        IsActive: true
      }
    ];
    const model = {
      rowIndex: 0,
      IsActive: false
    };
    const expected = [
      {
        IsActive: false
      },
      {
        IsActive: true
      }
    ];
    expect(updateActive(dataSource, model)).toEqual(expected);
  });

  it('Should update IsBlackList success', () => {
    const dataSource = [
      {
        IsBlackList: true
      },
      {
        IsBlackList: true
      }
    ];
    const model = {
      rowIndex: 0,
      IsBlackList: false
    };
    const expected = [
      {
        IsBlackList: false
      },
      {
        IsBlackList: true
      }
    ];
    expect(updateBlackList(dataSource, model)).toEqual(expected);
  });

  it('Should calculate number Of days between two dates correct', () => {
    const toDate = new Date();
    const fromDate = new Date('04-11-2019');
    expect(
      calculateNumberOfDaysBetweenTwoDates(fromDate, toDate) > 365
    ).toEqual(true);
  });

  it('Should build model TV list correct', () => {
    const state = {
      tvListReducer: {
        tvListExpression: [
          {
            value: 'abc',
            expression: {
              label: 'Contains',
              value: 1
            }
          }
        ]
      }
    };
    expect(buildModelTVList(state)).toEqual([
      { Label: 'Contains', Value: 'abc' }
    ]);
  });

  it('Should build model condition correct', () => {
    const state = {
      leagueSettingId: 123,
      tv: {
        hasTV: true,
        tv: 'tv'
      },
      round: [
        {
          value: 'round',
          expression: {
            value: 1,
            label: 'contains'
          }
        }
      ],
      division: [
        {
          value: 'division',
          expression: {
            value: 1,
            label: 'contains'
          }
        }
      ]
    };
    expect(buildModelCondition(state)).toEqual({
      Divisions: [{ Type: 'contains', Value: 'division' }],
      HasTV: true,
      LeagueSettingId: 123,
      Rounds: [{ Type: 'contains', Value: 'round' }],
      TV: 'tv'
    });
  });

  it('Should return false when all policy has write is false', () => {
    const policyData = {
      generalPolicy: {
        HasWrite: false
      },
      conditionPolicy: {
        HasWrite: false
      },
      lstNotePolicy: {
        HasWrite: false
      },
      tvListPolicy: {
        HasWrite: false
      }
    };

    expect(hasWriteOnPageMaxbetGeneral(policyData)).toEqual(false);
  });

  it('Should return true when has any policy has write is true', () => {
    const generalPolicy = {
      generalPolicy: {
        HasWrite: true
      }
    };

    const conditionPolicy = {
      generalPolicy: {
        HasWrite: false
      },
      conditionPolicy: {
        HasWrite: true
      }
    };

    const lstNotePolicy = {
      generalPolicy: {
        HasWrite: false
      },
      conditionPolicy: {
        HasWrite: false
      },
      lstNotePolicy: {
        HasWrite: true
      }
    };
    const tvListPolicy = {
      generalPolicy: {
        HasWrite: false
      },
      conditionPolicy: {
        HasWrite: false
      },
      lstNotePolicy: {
        HasWrite: false
      },
      tvListPolicy: {
        HasWrite: true
      }
    };

    expect(hasWriteOnPageMaxbetGeneral(generalPolicy)).toEqual(true);

    expect(hasWriteOnPageMaxbetGeneral(conditionPolicy)).toEqual(true);
    expect(hasWriteOnPageMaxbetGeneral(lstNotePolicy)).toEqual(true);
    expect(hasWriteOnPageMaxbetGeneral(tvListPolicy)).toEqual(true);
  });

  it('Should update league detail successfully', () => {
    const dataSource = [
      { Remark: 'remark', LstNote: 'Note' },
      { Remark: 'remark1', LstNote: 'Note1' }
    ];
    const extraModel = {
      rowIndex: 0,
      key: 'Remark',
      value: 'Carlos'
    };
    expect(updateLeagueDetail(dataSource, extraModel)).toEqual([
      { Remark: 'Carlos', LstNote: 'Note' },
      { Remark: 'remark1', LstNote: 'Note1' }
    ]);
  });

  it('Should remove league detail successfully', () => {
    const dataSource = [
      { Remark: 'remark', LstNote: 'Note' },
      { Remark: 'remark1', LstNote: 'Note1' }
    ];
    const extraModel = {
      rowIndex: 0
    };
    expect(removeLeagueDetail(dataSource, extraModel)).toEqual([
      { Remark: 'remark1', LstNote: 'Note1' }
    ]);
  });
});
