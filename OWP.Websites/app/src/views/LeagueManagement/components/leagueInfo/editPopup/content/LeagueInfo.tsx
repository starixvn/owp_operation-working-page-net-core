import { useAppSelector, useAppDispatch } from '../../app/hooks';
import Select from 'react-select-ts';
import { TOption } from '../../component/Type';
import { updateObject } from '../slice/editSettingSlice';

const LeagueInfo = (props: {
  leagueGroupId: number;
  league: string;
  shortLeague: string;
  blackList: boolean;
  noOfferLive: boolean;
}) => {
  const dispatch = useAppDispatch();
  const { leagueGroupId, league, shortLeague, blackList, noOfferLive } = props;
  const leagueGroupOptions = useAppSelector(
    state => state.addSettingSlice.leagueGroupOptions
  );


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

  const renderLeagueName = () => (
    <div className='content'>
      <div className='left'>
        League Name
        <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
      </div>
      <label style={{ marginTop: '7px' }}>{league}</label>
    </div>
  );

  const renderShortLeague = () => (
    <div className='content'>
      <div className='left'>
        Short League
        <p style={{ 'color': 'red', 'paddingLeft': '5px' }}>*</p>
      </div>
      <label style={{ marginTop: '7px' }}>{shortLeague}</label>
    </div>
  );

  const renderLeagueGroup = () => (
    <div className='content'>
      <div className='left'>League Group</div>
      <Select
        options={leagueGroupOptions}
        value={leagueGroupOptions.filter(x => x.value === leagueGroupId)}
        className='basic-multi-select'
        classNamePrefix='select'
        onChange={handleChangeLeagueGroup}
      />
    </div>
  );

  const renderLeagueType = () => (
    <div className='content'>
      <div className='left'>League Type</div>
      <div className='league--type__content'>
        <div className='checkbox form-check-league-type'>
          <input
            checked={blackList}
            id='chb-blacklist-league'
            type='checkbox'
            name='oldLeague'
            className='form-check-input'
            disabled
          />
          <label className='form-check-label' htmlFor='chb-blacklist-league'>
            Blacklist
          </label>
        </div>
        <div className='checkbox form-check-league-type'>
          <input
            checked={noOfferLive}
            id='chb-no-offer-live-league'
            type='checkbox'
            name='oldLeague'
            className='form-check-input'
            onChange={event => dispatch(updateObject({ property: 'noOfferLive', value: event.currentTarget.checked }))}
          />
          <label className='form-check-label' htmlFor='chb-no-offer-live-league'>
            No Offer Live
          </label>
        </div>
      </div>
    </div>
  );

  return (
    <div className='list'>
      <div className='title'>League Information</div>
      {renderLeagueName()}
      {renderShortLeague()}
      {renderLeagueGroup()}
      {renderLeagueType()}
    </div>
  );
};
export default LeagueInfo;
