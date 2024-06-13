import axios from 'axios';
import { useEffect, useState } from 'react';
import { baseApiUrl } from '../Constants';

const url = `${baseApiUrl}/api/AccountSetting/QuickMessage/get-quick-message`;

const useQueryMessageSuggestion = () => {
  const [suggestionSource, setSuggestionSource] = useState<
    Array<{
      shortcut: string;
      message: string;
    }>
  >([]);

  useEffect(() => {
    (async () => {
      const response = await axios.get(url);
      setSuggestionSource(response.data);
    })();
  }, []);

  return suggestionSource;
};

export default useQueryMessageSuggestion;
