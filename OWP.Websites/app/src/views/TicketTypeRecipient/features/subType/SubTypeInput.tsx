import { useState } from 'react';
import { Subtype } from '../TypeNative';
import { validateDuplicateSubtype } from '../../helper/validateDuplicateSubType';
import ToastrWrapper from '../../../components/ToastrWrapper';
type Prop = {
  onAddNew: (subtype: Subtype) => void;
  subtypes: Subtype[];
};

const SubTypeInput = (props: Prop) => {
  const { onAddNew, subtypes } = props;
  const [value, setValue] = useState<string>('');

  const handleSubmit = () => {
    value && handleAddNewSubType();
  };

  const handleAddNewSubType = () => {
    if (validateDuplicateSubtype(value, subtypes)) {
      onAddNew({
        subtypeId: 0,
        typeId: 0,
        subtypeName: value
      });
      setValue('');
    } else {
      ToastrWrapper.showError(
        `A subtype with this name <b>${value}</b> already exists. Please try again!`
      );
    }
  };

  return (
    <div className='new-subtype'>
      <label>subtype:</label>
      <input
        className='subtype-text-input'
        type='text'
        placeholder='Add subtype'
        value={value}
        maxLength={100}
        onChange={e => setValue(e.target.value)}
      />
      <button onClick={handleSubmit} className='button button-add'>
        Add
      </button>
    </div>
  );
};

export default SubTypeInput;
