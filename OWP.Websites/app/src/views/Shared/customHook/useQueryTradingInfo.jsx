import { useEffect } from 'react';
import useStableDispatch from './useStableDispatch';
import {
  getSoccerTradingPageInfo,
  getUserSettingServer
} from '../tradingPage/getTradingPageInfoThunk';

const useQueryTradingInfo = () => {
  const dispatch = useStableDispatch();
  useEffect(() => {
    dispatch(getSoccerTradingPageInfo());
    dispatch(getUserSettingServer());
  }, []);
};

export default useQueryTradingInfo;
