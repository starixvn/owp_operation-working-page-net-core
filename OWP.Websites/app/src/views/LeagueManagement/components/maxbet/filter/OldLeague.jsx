import { Input } from 'reactstrap';
import { useDispatch, useStore } from 'react-redux';
import { updateExpressionFilter } from '../../../actions/action';
import { ACTION_DISPLAY_OLDLEAGUE } from '../../../constants/action-type';

const OldLeague = () => {
  const store = useStore();
  const { displayOldLeague } = store.getState().displayOldLeagueReducer;
  const dispatch = useDispatch();
  const handleChangeDisplayOldLeague = event => {
    const value = event.target.checked;
    dispatch(
      updateExpressionFilter(ACTION_DISPLAY_OLDLEAGUE.UPDATE, { value })
    );
  };

  return (
    <div className='filter-control checkbox form-check'>
      <Input
        defaultChecked={displayOldLeague}
        id='chb-old-league'
        type='checkbox'
        name='oldLeague'
        className='form-check-input'
        onChange={handleChangeDisplayOldLeague}
      />
      <label className='form-check-label' htmlFor='chb-old-league'>
        Include Old Leagues
      </label>
    </div>
  );
};

export default OldLeague;
