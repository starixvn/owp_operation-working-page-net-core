import { ChangeEvent, useState } from 'react';
import { TLeagueName } from '../../typing/type';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';

const LeagueName = (props: TLeagueName) => {
  const [value, setValue] = useState<string | undefined>(undefined);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    setValue(newValue);
    props.onChange(newValue);
  };
  const errorMessage = useStore(state => state.errorMessage.leagueName);

  return (
    <div className='setting-league-name'>
      <div className='label'>
        League Name
        <span className='required-field'> *</span>
      </div>
      <input
        className='input-field'
        type='text'
        value={value}
        onChange={handleChange}
        maxLength={500}
        disabled={props.disabled}
      />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default LeagueName;
