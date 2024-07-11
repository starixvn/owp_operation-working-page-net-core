import axios from 'axios';
import {buildUrl, queryDataExcel} from './UtilsNative';
jest.mock('axios', ()=>({
  post: jest.fn()
}));

axios.post.mockReturnValue({});

describe('Utils under test', ()=>{
  beforeEach(()=>{
    jest.resetAllMocks();
  })
  it('should call axios post method', ()=>{
    const url='http://localhost/exports/';
    queryDataExcel(url,{})
    expect(axios.post).toBeCalledWith(url, {},{"responseType": "blob"});
  });

  it('should build Url correct', ()=>{
    const actual=buildUrl('exports');
    expect(actual).toEqual('/api/EventPicking/exports');
  })
});