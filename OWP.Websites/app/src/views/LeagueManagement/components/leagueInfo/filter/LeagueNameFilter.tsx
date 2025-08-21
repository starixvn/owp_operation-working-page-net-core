import { KeyboardEvent, useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { setLeagueHeaderFiler } from '../table/leagueInfoTableSlice/reducer';


const LeagueNameFilter = () => {
  const dispatch = useAppDispatch();
  const searchParams = new URLSearchParams(document.location.search);
  const owpLeagueName = searchParams.get('leagueName');
  const [value, setValue] = useState(owpLeagueName || '');
  const { leagueFilterHeader } = useAppSelector(state => state.leagueInfoTableSlice);

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      dispatch(setLeagueHeaderFiler({
        ...leagueFilterHeader,
        leagueName: value.trim()
      }));
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;
    setValue(newValue);
    dispatch(setLeagueHeaderFiler({
      ...leagueFilterHeader,
      leagueName: newValue.trim()
    }));
  };

  useEffect(() => {
    if (owpLeagueName) {
      setValue(owpLeagueName);
      dispatch(setLeagueHeaderFiler({
        ...leagueFilterHeader,
        leagueName: owpLeagueName.trim()
      }));
    }
  }, [owpLeagueName]);

  return (
    <div className='league-filter'>
      <span> League search</span>
      <input
        type='text'
        onKeyDown={handleKeyDown}
        value={value}
        onChange={handleInputChange}
      />
    </div>
  );
};

export default LeagueNameFilter;
