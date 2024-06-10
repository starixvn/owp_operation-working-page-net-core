import { useEffect } from 'react';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { useSelector } from 'react-redux';
import {
  QUERY_DELETE_LEAGUE_API_KEY,
  simpleApiQueryUrl
} from '../../const/const';
import { deleteLeague } from '../deletedLeague/deleteLeagueSlice';

const useDeletedLeague = (isDay = false) => {
  const stableDispatch = useStableDispatch();
  const model = useSelector(state => state.deletedLeague);
  const filterDate = useSelector(state => state.filter.filterDate);

  useEffect(() => {
    const filterDeleteModel = {
      url: simpleApiQueryUrl,
      apikey: QUERY_DELETE_LEAGUE_API_KEY,
      EventDate: filterDate,
      IsDay: isDay
    };
    filterDate && stableDispatch(deleteLeague(filterDeleteModel));
  }, [stableDispatch, filterDate, isDay]);
  return model;
};

export default useDeletedLeague;
