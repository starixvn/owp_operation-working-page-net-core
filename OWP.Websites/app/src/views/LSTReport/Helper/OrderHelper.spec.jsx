import helper from './OrderHelper';

describe('OrderHelper under test', () => {
  it('test orderMatchAsc', () => {
    const matches = [{ Match: 10 }, { Match: 5 }, { Match: 20 }];
    const actual = helper.orderMatchAsc(matches);
    const expected = [{ Match: 5 }, { Match: 10 }, { Match: 20 }];
    expect(actual).toEqual(expected);
  });

  it('test orderMatchAsc', () => {
    const matches = [{ Match: 10 }, { Match: 5 }, { Match: 20 }];
    const actual = helper.orderMatchDesc(matches);
    const expected = [{ Match: 20 }, { Match: 10 }, { Match: 5 }];
    expect(actual).toEqual(expected);
  });

  it('test orderSubColumnAsc', () => {
    const matches = [
      { type1: { HDP: 25 } },
      { type1: { HDP: 30 } },
      { type1: { HDP: 20 } },
      { type1: { HDP: '-' } }
    ];
    const actual = helper.orderSubColumnAsc(matches, 'type1', 'HDP');
    const expected = [
      { type1: { HDP: '-' } },
      { type1: { HDP: 20 } },
      { type1: { HDP: 25 } },
      { type1: { HDP: 30 } }
    ];
    expect(actual).toEqual(expected);
  });

  it('test orderSubColumnDesc', () => {
    const matches = [
      { type1: { HDP: 25 } },
      { type1: { HDP: 30 } },
      { type1: { HDP: 20 } },
      { type1: { HDP: '-' } }
    ];
    const actual = helper.orderSubColumnDesc(matches, 'type1', 'HDP');
    const expected = [
      { type1: { HDP: 30 } },
      { type1: { HDP: 25 } },
      { type1: { HDP: 20 } },
      { type1: { HDP: '-' } }
    ];
    expect(actual).toEqual(expected);
  });
});
