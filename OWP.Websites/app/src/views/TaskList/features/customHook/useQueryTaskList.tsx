import { useEffect } from 'react';
import { TQueryTaskList } from '../../type/TTaskListTable';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { taskListThunk } from '../table/tableThunk';

const useQueryTaskList = ({ eventDateTime }: TQueryTaskList) => {
  const dispatch = useStableDispatch();
  useEffect(() => {
    dispatch(
      taskListThunk({
        eventDateTime
      })
    );
  }, []);
};

export default useQueryTaskList;
