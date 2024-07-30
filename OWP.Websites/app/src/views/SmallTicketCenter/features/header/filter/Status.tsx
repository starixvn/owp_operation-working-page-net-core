import { MultiSelect } from 'react-multi-select-component';
import {STATUS} from '../../../const/const.json';
import { useAppDispatch } from '../../../app/hooksNative';
import store from '../../../app/storeNative';
import { useState } from 'react';
import { Option } from '../../../../Shared/MultipleSelection/type';
import { setFilterParam } from './filterSlice';

const Status = () => {
  const dispatch= useAppDispatch();
  const {filterInfo: {ticketStatuses}}= store.getState().filter.entities;
  
  const [selectedStatuses, setSelectedStatuses] = useState<Array<Option>>(STATUS.filter(
    x => x.value === ticketStatuses
  ));

  const handleChange=(event: Array<Option>)=>{
    setSelectedStatuses(event);
    dispatch(setFilterParam({
      key: 'ticketStatuses',
      value: event.map(x=>x.value).join(",")
    }));
  };

  return (
    <div className='multi-select-dropdown'>
    <div className='label'>Status</div>
    <MultiSelect
      className='status'
      options={STATUS}
      value={selectedStatuses}
      disableSearch
      ClearSelectedIcon={null}
      onChange={handleChange}
      labelledBy=''
    />
  </div>
  );
};

Status.propTypes = {
  
};

export default Status;