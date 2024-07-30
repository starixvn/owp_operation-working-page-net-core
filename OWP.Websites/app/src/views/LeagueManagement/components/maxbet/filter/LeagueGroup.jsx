import { useEffect, useCallback } from 'react';
import { useDispatch, useSelector, useStore } from 'react-redux';
import Select from 'react-select';
import MaxbetConst from '../../../constants/MaxbetConst';
import Loading from '../../../../components/Loading';
import { queryData, updateExpressionFilter } from '../../../actions/action';
import { selectLeagueGroup } from '../../../selector/SelectorMaxbetGeneral';
import {
  QUERY_LEAGUEGROUP,
  ACTION_LEAGUEGROUP_FILTER
} from '../../../constants/action-type';

const LeagueGroup = () => {
  const store = useStore();
  const dispatch = useDispatch();
  const model = useSelector(selectLeagueGroup);
  const handleChangeLeagueGroup = select => {
    dispatch(
      updateExpressionFilter(ACTION_LEAGUEGROUP_FILTER.UPDATE, { select })
    );
  };
  const stableDispatch = useCallback(dispatch, [dispatch]);
  useEffect(() => {
    stableDispatch(queryData(MaxbetConst.leagueGroupUrl, QUERY_LEAGUEGROUP));
  }, [stableDispatch]);
  const { leagueGroupBackup } = store.getState().leagueGroupReducer;
  if (model.leagueGroupPending) {
    return <Loading />;
  }
  return (
    <div className='filter-control'>
      <div className='label'>League Group</div>
      <Select
        options={model.leagueGroupOption}
        defaultValue={leagueGroupBackup}
        className='basic-multi-select'
        classNamePrefix='select'
        name='leagueGroup'
        onChange={handleChangeLeagueGroup}
      />
    </div>
  );
};

export default LeagueGroup;
