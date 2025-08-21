import { useAppSelector } from '../app/hooks';
import Select from 'react-select-ts';
import { TOption } from '../component/Type';

interface LeagueGroupFilterProps {
  onSearchValueChange: (value: string | null, field: string) => void;
}

const LeagueGroupFilter = ({ onSearchValueChange }: LeagueGroupFilterProps) => {
  const { leagueGroupOptions } = useAppSelector(state => state.editSettingSlice);
  const { leagueFilterHeader } = useAppSelector(state => state.leagueInfoTableSlice);
  
  const allOptions: TOption[] = [
    { value: 0, label: 'All' },
    ...leagueGroupOptions
  ];
  
  const handleChangeLeagueGroup = (selectedOption: TOption | null) => {
    const value = selectedOption?.value === 0 ? null : selectedOption?.value || null;
    onSearchValueChange(value?.toString() || null, 'leagueGroupId');
  };
  
  return (
    <div className='league-group-filter'>
      <span> League Group</span>
      <Select
        options={allOptions}
        value={allOptions.find(option => option.value === leagueFilterHeader.leagueGroupId) || null}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeLeagueGroup}
        placeholder="Select League Group..."
      />
    </div>
  );
};

export default LeagueGroupFilter;
