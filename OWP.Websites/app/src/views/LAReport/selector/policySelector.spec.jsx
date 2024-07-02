import { policySelector } from './policySelector';

describe('policySelector under test', () => {
  it('select policySelector ', () => {
    const state = {
      policyReducer: {
        laPolicy: 'hO no',
        pending: true
      }
    };
    expect(policySelector(state)).toEqual({
      laPolicy: 'hO no',
      pending: true
    });
  });
});
