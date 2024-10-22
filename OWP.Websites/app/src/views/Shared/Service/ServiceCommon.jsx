import axios from 'axios';
import Spinner from '../../components/Spinner';
import ToastrWrapper from '../../components/ToastrWrapper';
import { getMMDDYYYYStringFromDateTime } from '../DateTime/dateTimeHelper';

const queryServerTime = async url => {
  const response = await axios.get(url);
  return response.data;
};

const queryData = async (model, url) => {
  model.fromDate = getMMDDYYYYStringFromDateTime(model.fromDate);
  model.toDate = getMMDDYYYYStringFromDateTime(model.toDate);
  const response = await axios.post(url, model);
  return response.data;
};

const setAxiosInterceptors = () => {
  axios.defaults.withCredentials = true;
  axios.defaults.headers = {
    'X-Requested-With': 'XMLHttpRequest'
  };
  axios.interceptors.response.use(
    response => response,
    error => {
      const { response } = error;
      if (typeof response === 'undefined') {
        ToastrWrapper.showError('Occur error');
        return undefined;
      }
      if (response.status === 500) {
        if (response.data?.isAdminSiteUM) {
          ToastrWrapper.showInfo('AdminSite is unavailable');
        } else {
          ToastrWrapper.showError('Internal server error');
        }
        return undefined;
      }
      if (response.status === 404) {
        ToastrWrapper.showError('404 - Not found');
        return Promise.reject(error);
      }
      if (response.status === 401) {
        ToastrWrapper.showWarningSessionExpired();
        return undefined;
      }
      let message = '';
      if (response.data) {
        if (typeof response.data === 'object') {
          message = response.data.Message;
        } else {
          message = response.data;
        }
      } else if (response.Message) {
        message = response.Message;
      } else if (response.statusText) {
        message = response.responseText;
      }

      Spinner.hide();
      ToastrWrapper.showError(message);
      return Promise.reject(error);
    }
  );
};

export { queryServerTime, queryData, setAxiosInterceptors };
