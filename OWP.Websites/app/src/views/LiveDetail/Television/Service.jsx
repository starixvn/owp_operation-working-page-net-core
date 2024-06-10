import axios from 'axios';
import Utils from '../../Shared/Utils';
import Converter from '../../Shared/Converter';
import { buildQuerySimpleApi } from '../../Shared/Constants';
import { buildRequestURL } from '../buildUrlHelper';
const getSuggestionOptions = async keyword => {
  if (keyword.length < 2) {
    return null;
  }

  const postModel = {
    ApiKey: 'get-television-suggestion',
    Keyword: keyword
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  const data = response.data;
  return Converter.convertTVsToOptions(data);
};

const service = {
  getAll: () =>
    Utils.requestOrGetSessionStorage(
      'all-televisions',
      buildRequestURL('television/all')
    ),
  getSuggestionOptions
};

export default service;
