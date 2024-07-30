import { setAxiosInterceptors } from './ServiceCommon';
import axios from 'axios';
import Spinner from '../../components/Spinner';
import { noop } from 'lodash';

jest.mock('../../components/Spinner');

Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

describe('axios interceptor', () => {
  beforeAll(() => {
    return setAxiosInterceptors();
  });

  it('axios.interceptors fulfilled should be called', () => {
    const res = {
      response: { status: 400 }
    };
    const rejectedRes = axios.interceptors.response.handlers[0].fulfilled({
      ...res
    });

    expect(rejectedRes).toMatchObject(res);
  });

  it('axios.interceptors rejected should be called', () => {
    const rejectedRes = axios.interceptors.response.handlers[0].rejected({
      response: {
        statusText: 'NotFound',
        status: 404,
        data: { message: 'Page not found' }
      }
    });

    expect(rejectedRes).rejects.toMatchObject({
      response: {
        statusText: 'NotFound',
        status: 404,
        data: { message: 'Page not found' }
      }
    });
  });
});
