import { useAppSelector } from '../../app/hooks';
import Select from 'react-select-ts';
import { TOption } from '../../editPopup/EditType';

const LeagueInfoOfAdd = () => {
  const leagueGroupOptions = useAppSelector(
    state => state.addSettingSlice.leagueGroupOptions
  );

  const handleOnChangeLeagueName = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    console.log(event.target.value);
  };

  const handleChangeLeagueGroup = (selectedOption: TOption | null) => {
    const value = selectedOption?.value || 0;
    // if (leagueGroupId !== value) {
    //   dispatch(updateObject({ property: 'leagueGroupId', value }));
    //   dispatch(
    //     updateObject({
    //       property: 'leagueGroup',
    //       value: selectedOption?.label || ''
    //     })
    //   );
    // }
  };

  return (
    <div className='list'>
      <div className='title'>League Information</div>
      <div className='content'>
        <div className='left'>League Name</div>
        <input
          style={{ 'width': '500px' }}
          value={''}
          className='input'
          onChange={handleOnChangeLeagueName}
        />
      </div>
      <div className='content'>
        <div className='left'>League Group</div>
        <Select
          options={leagueGroupOptions}
          //value={leagueGroupOptions[0]}
          className='basic-multi-select'
          classNamePrefix='select'
          onChange={handleChangeLeagueGroup}
        />
      </div>
    </div>
  );
};
export default LeagueInfoOfAdd;
