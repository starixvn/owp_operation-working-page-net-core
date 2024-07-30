import { useEffect, useCallback } from 'react';
import WorkingShiftTable from './WorkingShiftTable';
import { useDispatch, useSelector } from 'react-redux';
import { workingShift } from './WorkingShiftSlice';
import { URL_QUERY_WORKING_SHIFT } from '../../const/const';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const WorkingShift = () => {
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const model = useSelector(state => state.workingShift);
  const filterDate = useSelector(state => state.filter.filterDate);

  useEffect(() => {
    const filterWorkingShiftModel = {
      url: URL_QUERY_WORKING_SHIFT,
      date: filterDate
    };
    stableDispatch(workingShift(filterWorkingShiftModel));
  }, [filterDate, stableDispatch]);

  if (
    isEqual(model.loading, REQUEST_STATUS.pending) ||
    model.workingShiftData === null
  ) {
    return <Loading />;
  }
  return <WorkingShiftTable workingShiftData={model.workingShiftData} />;
};
export default WorkingShift;
