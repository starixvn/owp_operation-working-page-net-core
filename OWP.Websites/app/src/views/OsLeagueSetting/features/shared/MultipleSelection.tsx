import { useState } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import { IMultipleSelectProps, Option } from '../../typing/type';

const flagBuildLabel = 3;
const MultipleSelection = (props: IMultipleSelectProps) => {
  const [selected, setSelected] = useState<Option[]>(props.value);
  const { label, isRequired, ...multiSelectProp } = props;

  const handleChange = (event: Option[]) => {
    const { onChange } = multiSelectProp;
    onChange && onChange(event);
    setSelected(event);
  };

  const buildSelectedThreeItems = (newSelected: Option[]) =>
    newSelected.reduce(
      (previous, current, index) =>
        index ? `${previous}, ${current.label}` : newSelected[index].label,
      newSelected[0].label
    );

  const customValueRenderer = (newSelected: Option[], _options: Option[]) => {
    if (!newSelected.length) {
      return 'Select...';
    }
    return newSelected.length <= flagBuildLabel
      ? buildSelectedThreeItems(newSelected)
      : `${newSelected.length}  items are selected`;
  };

  return (
    <div className='multiselect'>
      <div className='label'>
        {label}
        {isRequired && <span className='required-field'> *</span>}
      </div>
      <MultiSelect
        onChange={handleChange}
        valueRenderer={customValueRenderer}
        value={selected}
        options={props.options}
        ClearSelectedIcon={null}
        className='multi-select'
        disableSearch
        disabled={props.disabled}
        hasSelectAll
        labelledBy='Select...'
        overrideStrings={{
          selectAll: '-- Select All --'
        }}
      />
    </div>
  );
};

export default MultipleSelection;
