import { ChangeEvent, useState } from 'react';
import store from '../app/useStoreNative';

const User = () => {
  const { setFilterValue } = store.getState();
  const [value, setValue] = useState('');
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setValue(e.target.value);
    setFilterValue(e.target.value, 'userName');
  };

  return (
    <>
      <div className='label'>User</div>
      <input
        type='text'
        name='username'
        placeholder='Account Name or Nick Name'
        className='input-field'
        maxLength={100}
        value={value}
        onChange={handleChange}
      />
    </>
  );
};

export default User;