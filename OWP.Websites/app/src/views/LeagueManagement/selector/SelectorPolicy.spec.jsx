import { selectPolicy } from './SelectorPolicy';

describe('test selector', () => {
  it('select selectPolicy ', () => {
    const state = {
      policyReducer: {
        policyPending: true
      }
    };
    expect(selectPolicy(state)).toEqual({ policyPending: true });
  });
});
