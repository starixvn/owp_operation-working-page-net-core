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
    ApiKey: 'get-other-provider-suggestion',
    Keyword: keyword
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  const data = response.data;
  return Converter.convertOtherProvidersToOptions(data);
};

const service = {
  getAll: () =>
    Utils.requestOrGetSessionStorage(
      'all-other-providers',
      buildRequestURL('other-provider/all')
    ),
  getSuggestionOptions
};

export default service;
