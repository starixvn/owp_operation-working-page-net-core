import { ChangeEvent, useState } from 'react';
import useStore from '../../app/store';
import { IRemark } from '../../typing/type';
import ErrorMessage from './ErrorMessage';

const Remark = (props: IRemark) => {
  const { onChange, disabled } = props;
  const [value, setValue] = useState(props.value);
  const errorMessage = useStore(state => state.errorMessage.remark);

  const handleChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = event.target.value;
    setValue(newValue);
    onChange && onChange(newValue);
  };

  return (
    <div className='remark-content'>
      <label htmlFor='remark'>Remark</label>
      <div>
        <textarea
          name='remark'
          id='remark'
          rows={5}
          value={value}
          maxLength={2000}
          disabled={disabled}
          onChange={handleChange}></textarea>
      </div>
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default Remark;
