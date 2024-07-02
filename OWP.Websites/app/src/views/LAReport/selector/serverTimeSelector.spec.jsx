import { serverTimeSelector } from './serverTimeSelector';

describe('serverTimeSelector under test', () => {
  it('select serverTimeSelector ', () => {
    const state = {
      serverTimeReducer: {
        serverTime: '28/12/2020',
        pending: true
      }
    };
    expect(serverTimeSelector(state)).toEqual({
      serverTime: '28/12/2020',
      pending: true
    });
  });
});
