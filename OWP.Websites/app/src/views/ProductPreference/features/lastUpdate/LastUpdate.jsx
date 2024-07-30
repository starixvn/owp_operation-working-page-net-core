import { useState, useEffect } from 'react';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import axios from 'axios';
import { buildLastUpdateUrl } from '../../utils';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';

const LastUpdate = () => {
  const [loading, setLoading] = useState(REQUEST_STATUS.pending);
  const [lastUpdateInfo, setLastUpdateInfo] = useState({});
  useEffect(() => {
    (async () => {
      const url = buildLastUpdateUrl();
      const response = await axios.get(url);
      setLastUpdateInfo(response.data);
      setLoading(false);
    })();
  }, []);

  if (isEqual(loading, REQUEST_STATUS.loading)) {
    return <Loading />;
  }

  const { date, username } = lastUpdateInfo;

  return (
    <div className='last-update'>
      {date && `Last Updated at ${getLocaleDateTime(date, 'LLL')} by ${username}`}
    </div>
  );
};

export default LastUpdate;
