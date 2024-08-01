import { KeyboardEvent, useEffect, useState } from 'react';
import { useAppDispatch } from '../app/hooks';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import { setLeagueHeaderFiler } from '../table/leagueInfoTableSlice/reducer';

const LeagueNameFilter = () => {
  const dispatch = useAppDispatch();
  const searchParams = new URLSearchParams(document.location.search);
  const owpLeagueName = searchParams.get('leagueName');
  const [value, setValue] = useState(owpLeagueName || '');
  const isEqualLeague = !!owpLeagueName;

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    e.key === 'Enter' && handleEnter();
  };

  const handleEnter = async () => {
    dispatch(setLeagueHeaderFiler(value));
    await dispatch(fetchLeagueSettingInfo(value, false));
  };

  useEffect(() => {
    dispatch(setLeagueHeaderFiler(value));
    dispatch(fetchLeagueSettingInfo(value, isEqualLeague));
  }, [owpLeagueName]);

  return (
    <div className='league-filter'>
      <span> League search</span>
      <input
        type='text'
        onKeyDown={handleKeyDown}
        value={value}
        onChange={e => setValue(e.target.value)}></input>
    </div>
  );
};

export default LeagueNameFilter;
