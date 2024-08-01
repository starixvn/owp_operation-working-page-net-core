import Select from 'react-select-ts';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { TOption } from '../EditType';
import { updateObject } from '../slice/editSettingSlice';

const Header = (props: { leagueGroupId: number; league: string }) => {
  const leagueGroupOptions = useAppSelector(
    state => state.editSettingSlice.leagueGroupOptions
  );
  const { leagueGroupId, league } = props;
  const dispatch = useAppDispatch();

  const handleChangeLeagueGroup = (selectedOption: TOption | null) => {
    const value = selectedOption?.value || 0;
    if (leagueGroupId !== value) {
      dispatch(updateObject({ property: 'leagueGroupId', value }));
      dispatch(
        updateObject({
          property: 'leagueGroup',
          value: selectedOption?.label || ''
        })
      );
    }
  };
  return (
    <div className='header'>
      <span>{league}</span>
      <Select
        options={leagueGroupOptions}
        value={leagueGroupOptions.filter(x => x.value === leagueGroupId)}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeLeagueGroup}
      />
    </div>
  );
};

export default Header;
