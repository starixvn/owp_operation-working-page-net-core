import { useEffect } from 'react';
import useStableDispatch from './useStableDispatch';
import { getSoccerTradingPageInfo } from '../tradingPage/getTradingPageInfoThunk';

const useGetSoccerTradingPageInfo = () => {
  const dispatch = useStableDispatch();
  useEffect(() => {
    dispatch(getSoccerTradingPageInfo());
  }, []);
};

export default useGetSoccerTradingPageInfo;
