import {
  buildLocalStoreKey,
  buildOptionDropdownToggleColumn,
  columnExists
} from './toggleColumnHelper';

describe('toggleColumnHelper', () => {
  it('Should return localstoreKey correct', () => {
    const policyData = {
      generalPolicy: {
        Username: 'sa',
        HasRead: true
      },
      conditionPolicy: {
        HasRead: true
      },
      lstNotePolicy: {
        HasRead: true
      },
      tvListPolicy: {
        HasRead: true
      }
    };
    const expected =
      'sa_LeagueManagement_General_LeagueManagement_EditConditions_LeagueManagement_LSTNote_LeagueManagement_TVList';
    const actual = buildLocalStoreKey(policyData);
    expect(actual).toEqual(expected);
  });

  it('Should return option toggle column correct when has permission read lst note', () => {
    const policyData = {
      lstNotePolicy: {
        HasRead: true
      }
    };
    const expected = [
      { value: 'remark', label: 'Remark' },
      { value: 'lstNote', label: 'LST Note' },
      { value: 'spread', label: 'Spread' },
      { value: 'maxbetWeekday', label: 'Max Bet - Weekday' },
      { value: 'maxbetWeekend', label: 'Max Bet - Weekend' },
      { value: 'maxbetFT1X2', label: '1X2' }
    ];
    const actual = buildOptionDropdownToggleColumn(policyData);
    expect(actual).toEqual(expected);
  });

  it('Should return option toggle column correct when has not permission read lst note', () => {
    const policyData = {
      lstNotePolicy: {
        HasRead: false
      }
    };
    const expected = [
      { value: 'remark', label: 'Remark' },
      { value: 'spread', label: 'Spread' },
      { value: 'maxbetWeekday', label: 'Max Bet - Weekday' },
      { value: 'maxbetWeekend', label: 'Max Bet - Weekend' },
      { value: 'maxbetFT1X2', label: '1X2' }
    ];
    const actual = buildOptionDropdownToggleColumn(policyData);
    expect(actual).toEqual(expected);
  });

  it('Should return true when value exists', () => {
    const container = ['One', 'Two', 'Three'];
    const exists = columnExists(container, 'One');
    expect(exists).toEqual(true);
  });

  it('Should return true when value not exists', () => {
    const container = ['One', 'Two', 'Three'];
    const exists = columnExists(container, 'Four');
    expect(exists).toEqual(false);
  });
});
