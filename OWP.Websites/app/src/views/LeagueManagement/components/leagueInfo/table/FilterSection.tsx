import LeagueNameFilter from '../filter/LeagueNameFilter';
import LeagueGroupFilter from '../filter/LeagueGroupFilter';
import { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { fetchLeagueSettingInfo } from './leagueInfoTableSlice/leagueInfoThunk';
import { setLeagueHeaderFiler } from './leagueInfoTableSlice/reducer';
import { Button } from 'reactstrap';
import AddLeagueSettingButton from '../filter/AddLeagueSettingButton';
import { getLeagueGroupOptionsThunk } from '../component/commonThunk';
import BlackListFilter from '../filter/BlackListFilter';
import OfferLiveFilter from '../filter/OfferLiveFilter';

const FilterSection = () => {
  const dispatch = useAppDispatch();
  const [searchValues, setSearchValues] = useState({
    leagueName: '',
    leagueGroupId: null as number | null
  });
  const { leagueFilterHeader } = useAppSelector(state => state.leagueInfoTableSlice);
  const { leagueGroupOptions } = useAppSelector(state => state.editSettingSlice);
  
  
  const handleSearch = async () => {
    await dispatch(fetchLeagueSettingInfo(leagueFilterHeader.leagueName, false));
  };

  useEffect(() => {
    !leagueGroupOptions.length && dispatch(getLeagueGroupOptionsThunk());
  }, [dispatch, leagueGroupOptions]);

  return (
    <div className='filter-section'>
      <LeagueNameFilter  />
      <LeagueGroupFilter  />
      <BlackListFilter  />
      <OfferLiveFilter  />
      <div className='filter-section-button'>
        <Button className='btn-search' onClick={handleSearch}>
          Search
        </Button>
        <AddLeagueSettingButton />
      </div>
    </div>
  );
};

export default FilterSection;
