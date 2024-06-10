import Select from 'react-select';
import { useDispatch, useStore } from 'react-redux';
import { updateExpressionFilter } from '../../../actions/action';
import { ACTION_CATEGORY_FILTER } from '../../../constants/action-type';
import MaxbetConst from '../../../constants/MaxbetConst';

const LeagueCategory = () => {
  const store = useStore();
  const { leagueCategory } = store.getState().leagueCategoryReducer;
  const dispatch = useDispatch();
  const handleChangeLeaguecCategory = select => {
    dispatch(updateExpressionFilter(ACTION_CATEGORY_FILTER.UPDATE, { select }));
  };
  return (
    <div className='filter-control'>
      <div className='label'>Category</div>
      <Select
        options={MaxbetConst.leagueCategoryOption}
        defaultValue={leagueCategory}
        className='basic-multi-select'
        classNamePrefix='select'
        name='leagueCategory'
        onChange={handleChangeLeaguecCategory}
      />
    </div>
  );
};

export default LeagueCategory;
