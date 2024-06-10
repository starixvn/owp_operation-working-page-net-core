import { useEffect } from 'react';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { isEqual } from 'lodash';
import { formatTimeOsLastUpdate } from '../../../Shared/DateTime/dateTimeHelper';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import getLastUpdateThunkNative from './getLastUpdateThunkNative';

type loadingType = 'idle' | 'pending' | 'succeeded' | 'failed';

interface LastUpdateInfo {
  loading: loadingType;
  entity: {
    date?: string;
    username?: string;
  };
}

const LastUpdated = (): JSX.Element => {
  const dispatch = useAppDispatch();
  const lastUpdateInfo = useAppSelector(
    state => state.lastUpdate
  ) as LastUpdateInfo;
  const { loading, entity } = lastUpdateInfo;

  useEffect(() => {
    dispatch(getLastUpdateThunkNative());
  }, [dispatch]);

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  const { date, username } = entity;

  return (
    <div className='last-update'>
      Last Updated at {formatTimeOsLastUpdate(date)} (GMT+8) by {username}
    </div>
  );
};

export default LastUpdated;
