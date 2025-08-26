import Select from 'react-select-ts';
import { TOption } from '../component/Type';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { setLeagueHeaderFiler } from '../table/leagueInfoTableSlice/reducer';

const LeagueGroupFilter = () => {
  const dispatch = useAppDispatch();
  const { leagueGroupOptions } = useAppSelector(state => state.editSettingSlice);
  const { leagueFilterHeader } = useAppSelector(state => state.leagueInfoTableSlice);
  const allOptions: TOption[] = [
    { value: null, label: 'All' },
    ...leagueGroupOptions
  ];

  const handleChangeLeagueGroup = (selectedOption: TOption | null) => {
    const value = selectedOption?.value;
    dispatch(setLeagueHeaderFiler({
      ...leagueFilterHeader,
      leagueGroupId: value
    }));
  };
  
  return (
    <div className='league-group-filter'>
      <span> League Group</span>
      <Select
        options={allOptions}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeLeagueGroup}
        placeholder="Select League Group..."
        value={leagueFilterHeader.leagueGroupId === null 
          ? allOptions[0] 
          : allOptions.find(option => option.value === leagueFilterHeader.leagueGroupId) || null}
      />
    </div>
  );
};

export default LeagueGroupFilter;
