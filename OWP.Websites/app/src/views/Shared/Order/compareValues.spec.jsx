import compareValues from './compareValues';

describe('Dynamic oder', () => {
  it('return zero when not pass params key', () => {
    const compareValue = compareValues()(1, 3);
    expect(compareValue).toEqual(0);
  });

  it('return 1 when oder ASC and value source greater than value dest', () => {
    const source = {
      number: 1
    };

    const dest = {
      number: 0
    };
    const compareValue = compareValues('number', 'asc')(source, dest);
    expect(compareValue).toEqual(1);
  });

  it('return -1 when oder DESC and value source greater than value dest', () => {
    const source = {
      number: 1
    };

    const dest = {
      number: 0
    };
    const compareValue = compareValues('number', 'desc')(source, dest);
    expect(compareValue).toEqual(-1);
  });

  it('return 1 when oder ASC , both value source and value dest is string', () => {
    const source = {
      name: 'b'
    };

    const dest = {
      name: 'a'
    };
    const compareValue = compareValues('name', 'asc')(source, dest);
    expect(compareValue).toEqual(1);
  });

  it('return -1 when oder DESC , both value source and value dest is string', () => {
    const source = {
      name: 'b'
    };

    const dest = {
      name: 'a'
    };
    const compareValue = compareValues('name', 'desc')(source, dest);
    expect(compareValue).toEqual(-1);
  });
});
