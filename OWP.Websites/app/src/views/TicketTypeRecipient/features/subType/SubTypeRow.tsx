import { isEqual } from 'lodash';
import { useState } from 'react';
import { Subtype } from '../TypeNative';

type Prop = {
  index: number;
  onUpdateSubtype: (value: Required<string>, index: number) => void;
} & Subtype;

const SubTypeRow = (props: Prop) => {
  const { subtypeName, index, onUpdateSubtype } = props;

  const [value, setValue] = useState(subtypeName);

  const handleBlur = () => {
    !isEqual(value, subtypeName) && onUpdateSubtype(value, index);
  };

  return (
    <div className='control-subtype'>
      <input
        className='subtype-text-input'
        type='text'
        value={value}
        maxLength={100}
        onBlur={handleBlur}
        onChange={e => setValue(e.target.value)}
      />
      {!value && <span className='subtype-error'>This field is required</span>}
    </div>
  );
};

export default SubTypeRow;
