import { useDispatch, useStore } from 'react-redux';
import { Input } from 'reactstrap';
import { updateExpressionFilter } from '../../../actions/action';
import { ACTION_LEAGUE_FILTER } from '../../../constants/action-type';

const League = () => {
  const store = useStore();
  const { league } = store.getState().leagueReducer;
  const dispatch = useDispatch();
  const handleChangeLeague = event => {
    const { value } = event.target;
    dispatch(updateExpressionFilter(ACTION_LEAGUE_FILTER.UPDATE, { value }));
  };
  return (
    <div className='filter-control'>
      <div className='label'>League</div>
      <Input
        type='text'
        name='league'
        className='input-field'
        defaultValue={league}
        onChange={handleChangeLeague}
        maxLength='100'
      />
    </div>
  );
};

export default League;
