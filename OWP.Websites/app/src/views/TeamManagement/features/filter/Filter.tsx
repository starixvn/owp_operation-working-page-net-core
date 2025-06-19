import Select from 'react-select';
import { useState } from 'react';
import { Option } from 'react-multi-select-component';
import { Input } from 'reactstrap';
import { useAppDispatch } from '../../app/hookNative';
import { getLeagues } from '../table/tableThunk';
import { LEAGUE_GROUP_OPTION } from '../../Const.json';

const Filter = () => {
  const dispatch = useAppDispatch();
  const [selectedLeagueGroup, setSelectedLeagueGroup] = useState<Option>(LEAGUE_GROUP_OPTION[0]);
  const [leagueName, setLeagueName] = useState<string>('');
  const [teamName, setTeamName] = useState<string>('');

  const handleSearch = () => {
    dispatch(getLeagues({
      leagueName,
      leagueGroup: selectedLeagueGroup.value,
      teamName
    }));
  };

  const handleChangeLeagueGroup = (selectedOption: Option) => {
    setSelectedLeagueGroup(selectedOption);
  };

  const handleChangeLeague = (event: React.ChangeEvent<HTMLInputElement>) => {
    setLeagueName(event.target.value);
  };

  const handleChangeTeam = (event: React.ChangeEvent<HTMLInputElement>) => {
    setTeamName(event.target.value);
  };

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control'>
          <div className='label'>League Name</div>
          <Input
            type='text'
            name='league'
            className='input-field'
            value={leagueName}
            onChange={handleChangeLeague}
          />
        </div>

        <div className='filter-control'>
          <div className='label'>League Group</div>
          <Select
            value={selectedLeagueGroup}
            className='basic-multi-select'
            classNamePrefix='select'
            options={LEAGUE_GROUP_OPTION}
            onChange={handleChangeLeagueGroup}
          />
        </div>

        <div className='filter-control'>
          <div className='label'>Team Name</div>
          <Input
            type='text'
            name='team'
            className='input-field'
            value={teamName}
            onChange={handleChangeTeam}
          />
        </div>

        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleSearch}>
            Search
          </button>
        </div>
      </div>
    </div>
  );
};

export default Filter;
