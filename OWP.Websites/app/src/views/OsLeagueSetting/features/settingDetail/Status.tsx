import { Switch } from '@mui/material';
import { ChangeEvent, useState } from 'react';
import { IStatus } from '../../typing/type';

const Status = (props: IStatus) => {
  const { isActive, onChange } = props;
  const [value, setValue] = useState(isActive);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { checked } = event.target;
    setValue(checked);
    onChange && onChange(checked);
  };

  return (
    <div className='setting-status'>
      <span className='status'>Active</span>
      <Switch
        data-testid='status-testid'
        color='primary'
        checked={value}
        onChange={handleChange}
      />
    </div>
  );
};

export default Status;
