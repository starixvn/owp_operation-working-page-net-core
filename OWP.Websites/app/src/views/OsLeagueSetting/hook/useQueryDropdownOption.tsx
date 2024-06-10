import { useEffect, useState } from 'react';
import { Option } from '../typing/type';
import {
  NET_CORE_QUERY_SIMPLE_API,
  QUERY_SIMPLE_API
} from '../../Shared/Constants';
import Utils from '../../Shared/Utils';
import { TDropdownOption } from '../typing/queryOptionsType';

const url =
  process.env.NODE_ENV === 'development'
    ? `${NET_CORE_QUERY_SIMPLE_API}`
    : `${QUERY_SIMPLE_API}`;

const useQueryDropdownOption = (props: TDropdownOption) => {
  const { apiKey, localStorageKey } = props;

  const [loading, setLoading] = useState(true);
  const [options, setOptions] = useState<Option[]>([]);

  useEffect(() => {
    (async () => {
      const option = await Utils.requestOrGetLocalStorage({
        url,
        apiKey,
        localStoreKey: localStorageKey
      });
      setOptions(option);
      setLoading(false);
    })();
  }, [apiKey, localStorageKey]);

  return {
    loading,
    options
  };
};

export default useQueryDropdownOption;
