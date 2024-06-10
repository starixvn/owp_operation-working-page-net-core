import useStore from '../../app/store';
import { MultiSelect } from 'react-multi-select-component';

const UserStatus = () => {
  const userStatusOption = useStore(state => state.userStatusOption);
  const statusSelected = useStore(state => state.userStatusSelected);
  const setStatusSelected = useStore(state => state.setUserStatusSelected);

  return (
    <div className='multi-select-dropdown'>
      <div className='label'>Account Type</div>
      <MultiSelect
        labelledBy='Select Account Type'
        className='status'
        options={userStatusOption}
        value={statusSelected}
        disableSearch
        ClearSelectedIcon={null}
        onChange={setStatusSelected}
      />
    </div>
  );
};

export default UserStatus;
