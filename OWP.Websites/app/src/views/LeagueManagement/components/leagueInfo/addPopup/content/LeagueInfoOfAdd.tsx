import { useAppSelector, useAppDispatch } from '../../app/hooks';
import Select from 'react-select-ts';
import { TOption } from '../../component/Type';
import { updateObjectAddPopup } from '../slice/addSettingSlice';
const LeagueInfoOfAdd = () => {
  const dispatch = useAppDispatch();

  const leagueGroupOptions = useAppSelector(
    state => state.addSettingSlice.leagueGroupOptions
  );

  const handleOnChangeLeagueName = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const value = event.target.value;

  };

  const handleChangeLeagueGroup = (selectedOption: TOption | null) => {
    const value = selectedOption?.value || 0;
    dispatch(updateObjectAddPopup({ property: 'leagueGroupId', value }));
    dispatch(
      updateObjectAddPopup({
        property: 'leagueGroup',
        value: selectedOption?.label || ''
      })
    );
  };


  return (
    <div className='list'>
      <div className='title'>League Information</div>
      <div className='content'>
        <div className='left'>
          League Name
          <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
        </div>

        <input
          style={{ 'width': '500px' }}
          className='input'
          onBlur={event => dispatch(updateObjectAddPopup({ property: 'league', value: event.target.value }))}
        />
      </div>
      <div className='content'>
        <div className='left'>League Group</div>
        <Select
          options={leagueGroupOptions}
          className='basic-multi-select'
          classNamePrefix='select'
          onChange={handleChangeLeagueGroup}
        />
      </div>
      <div className='content'>
        <div className='left'>League Type</div>
        <div className='checkbox form-check-league-type'>
          <input

            id='chb-no-offer-live-league'
            type='checkbox'
            name='oldLeague'
            className='form-check-input'
            onChange={event => dispatch(updateObjectAddPopup({ property: 'noOfferLive', value: event.currentTarget.checked }))}
          />
          <label className='form-check-label' htmlFor='chb-no-offer-live-league'>
            No Offer Live
          </label>
        </div>
      </div>
    </div>
  );
};
export default LeagueInfoOfAdd;
