import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { updateObjectAddPopup } from '../slice/addSettingSlice';

const LeagueInfoOfAdd = () => {
  const dispatch = useAppDispatch();
  const { league } = useAppSelector(
    state => state.addSettingSlice
  );

  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    dispatch(updateObjectAddPopup({ property: 'league', value: event.target.value }));
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
          onChange={handleOnChange}
          value={league}
        />
      </div>
    </div>
  );
};
export default LeagueInfoOfAdd;
