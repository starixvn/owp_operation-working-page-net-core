import { reportSelector } from './reportSelector';

describe('reportSelector under test', () => {
  it('select reportSelector ', () => {
    const state = {
      reportReducer: {
        pending: true,
        isDisplayTable: false,
        laInfo: '',
        orderType: '',
        columnId: '',
        isClearAll: false,
        keyWordLAColumn: '',
        keyWordSectionColumn: ''
      }
    };
    expect(reportSelector(state)).toEqual({
      pending: true,
      isDisplayTable: false,
      laInfo: '',
      orderType: '',
      columnId: '',
      isClearAll: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: ''
    });
  });
});
