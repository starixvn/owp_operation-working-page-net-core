import { useState } from 'react';
import Select from 'react-select-ts';
import { ISelection, Option } from '../../typing/type';

const Selection = (props: ISelection) => {
  const { disabled, label, isRequired, onChange, defaultValue, options } = props;
  const [selected, setSelected] = useState<Option | null>(defaultValue);

  const handleChange = (event: Option | null) => {
    const customlabel = event?.value
      ? event.label
      : '';

    const newValue = {
      ...event,
      label: customlabel
    } as Option;

    setSelected(newValue);
    onChange &&
      event &&
      onChange(event.value);
  };

  return (
    <div>
      <div className='label'>
        {label}
        {isRequired && <span className='required-field'> *</span>}
      </div>
      <Select
        className='select-field'
        options={options}
        onChange={handleChange}
        value={selected}
        isDisabled={disabled}
      />
    </div>
  );
};

export default Selection;
