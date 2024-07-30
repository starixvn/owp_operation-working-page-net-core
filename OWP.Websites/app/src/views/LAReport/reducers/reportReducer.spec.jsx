import {
  QUERY_LA,
  SEARCH,
  CLEAR_ALLFILTERS,
  ORDER
} from '../constants/action-type';
import reportReducer from './reportReducer';

describe('reportReducer under test', () => {
  it('should return the initial state', () => {
    expect(reportReducer(undefined, {})).toEqual({
      pending: false,
      laInfo: [],
      laInfoOriginal: [],
      isDisplayTable: false,
      isClearAll: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: '',
      orderType: '',
      columnId: ''
    });
  });
  it('should handle QUERY_LA.PENDING', () => {
    expect(
      reportReducer(undefined, {
        type: QUERY_LA.PENDING
      })
    ).toEqual({
      pending: true,
      laInfo: [],
      laInfoOriginal: [],
      isDisplayTable: false,
      isClearAll: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: '',
      orderType: '',
      columnId: ''
    });
  });
  it('should handle QUERY_LA.SUCCESS', () => {
    const data = {
      laInfo: '123',
      laInfoOriginal: '123',
      pending: false,
      isDisplayTable: true
    };
    const dataRecived = {
      columnId: '',
      isClearAll: false,
      isDisplayTable: true,
      keyWordLAColumn: '',
      keyWordSectionColumn: '',
      laInfo: {
        isDisplayTable: true,
        laInfo: '123',
        laInfoOriginal: '123',
        pending: false
      },
      laInfoOriginal: {
        isDisplayTable: true,
        laInfo: '123',
        laInfoOriginal: '123',
        pending: false
      },
      orderType: '',
      pending: false
    };
    const respons = reportReducer(undefined, {
      type: QUERY_LA.SUCCESS,
      data
    });
    expect(respons).toEqual(dataRecived);
  });
  it('should handle SEARCH, with columnId=Section, then keyWordSectionColumn will have keyword  ', () => {
    const payload = {
      columnId: 'Section',
      keyWord: '123'
    };
    const dataRecived = {
      columnId: '',
      isClearAll: false,
      isDisplayTable: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: '123',
      laInfo: [],
      laInfoOriginal: [],
      orderType: '',
      pending: false
    };
    const respons = reportReducer(undefined, {
      type: SEARCH,
      payload
    });
    expect(respons).toEqual(dataRecived);
  });
  it('should handle SEARCH, with columnId # Section, then keyWordLAColumn will have keyword ', () => {
    const payload = {
      columnId: 'else',
      keyWord: '123'
    };
    const dataRecived = {
      columnId: '',
      isClearAll: false,
      isDisplayTable: false,
      keyWordLAColumn: '123',
      keyWordSectionColumn: '',
      laInfo: [],
      laInfoOriginal: [],
      orderType: '',
      pending: false
    };
    const respons = reportReducer(undefined, {
      type: SEARCH,
      payload
    });
    expect(respons).toEqual(dataRecived);
  });
  it('should handle CLEAR_ALLFILTERS', () => {
    const isClearAll = true;

    const dataRecived = {
      columnId: '',
      isClearAll: true,
      isDisplayTable: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: '',
      laInfo: [],
      laInfoOriginal: [],
      orderType: '',
      pending: false
    };
    const respons = reportReducer(undefined, {
      type: CLEAR_ALLFILTERS,
      isClearAll
    });
    expect(respons).toEqual(dataRecived);
  });
  it('should handle ORDER', () => {
    const payload = { columnId: '1', orderType: '2' };

    const dataRecived = {
      columnId: '1',
      isClearAll: false,
      isDisplayTable: false,
      keyWordLAColumn: '',
      keyWordSectionColumn: '',
      laInfo: [],
      laInfoOriginal: [],
      orderType: '2',
      pending: false
    };
    const respons = reportReducer(undefined, {
      type: ORDER,
      payload
    });
    expect(respons).toEqual(dataRecived);
  });
});
