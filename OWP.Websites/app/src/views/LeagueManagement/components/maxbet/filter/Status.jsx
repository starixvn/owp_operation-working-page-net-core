import MaxbetConst from '../../../constants/MaxbetConst';
import Select from 'react-select';
import { useDispatch, useStore } from 'react-redux';
import { updateExpressionFilter } from '../../../actions/action';
import { ACTION_STATUS_FILTER } from '../../../constants/action-type';

const Status = () => {
  const store = useStore();
  const { status } = store.getState().statusReducer;
  const dispatch = useDispatch();
  const handleChangeStatus = select => {
    dispatch(updateExpressionFilter(ACTION_STATUS_FILTER.UPDATE, { select }));
  };

  return (
    <div className='filter-control'>
      <div className='label'>Status</div>
      <Select
        defaultValue={status}
        options={MaxbetConst.status}
        className='basic-multi-select'
        classNamePrefix='select'
        name='status'
        onChange={handleChangeStatus}
      />
    </div>
  );
};

export default Status;
