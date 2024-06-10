import { isEqual } from 'lodash';
import { useCallback, useEffect, useState } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import { IMultipleSelectProps, Option } from './type';

const CLASS_NAME_IS_SHOW = 'is-show';
const MultipleSelection = (props: IMultipleSelectProps) => {
  const [selected, setSelected] = useState<Option[]>(props.value);
  const {
    label,
    isRequired,
    onValueRenderer,
    onShowDropdownListOption,
    disableSearch,
    ...multiSelectProp
  } = props;
  const [className, setClassName] = useState<string | null>(null);
  const handleChange = useCallback((event: Option[]) => {
    setSelected(event);
  }, []);

  useEffect(() => {
    setSelected(props.value);
  }, [props.value]);

  const handleValueRenderer = (newSelected: Option[], _options: Option[]) => {
    if (onValueRenderer) {
      return onValueRenderer(newSelected, _options);
    }

    if (!newSelected.length) {
      return 'Select...';
    }

    if (isEqual(newSelected.length, props.options.length)) {
      return 'All';
    }
    return newSelected.reduce(
      (previous, current, index) =>
        index ? `${previous}, ${current.label}` : newSelected[index].label,
      newSelected[0].label
    );
  };

  const handleMenuToggle = (expand: boolean) => {
    const newClassName = expand ? CLASS_NAME_IS_SHOW : '';

    setClassName(newClassName);
    const { onChange } = multiSelectProp;
    !expand && onChange && onChange(selected);
  };

  useEffect(() => {
    onShowDropdownListOption &&
      className !== null &&
      onShowDropdownListOption(isEqual(className, CLASS_NAME_IS_SHOW));
  }, [className, onShowDropdownListOption]);

  const filterOptions = (options: Option[], filterKey: string) => {
    if (!filterKey) {
      setSelected(selected);
      return options;
    }

    const regex = new RegExp(filterKey, "i");
    const result = options.filter((x: Option) => x.label?.match(regex));
    setSelected(result);

    return result;
  };

  return (
    <div className={`multi-select-dropdown ${props.className} ${className}`}>
      <div className='label'>
        {label}
        {isRequired && <span className='required-field'> *</span>}
      </div>
      <MultiSelect
        onChange={handleChange}
        valueRenderer={handleValueRenderer}
        value={selected}
        options={props.options}
        ClearSelectedIcon={null}
        className={`multi-select ${props.disabled ? 'disable-text' : ''}`}
        disableSearch={props.disableSearch ?? true}
        disabled={props.disabled}
        hasSelectAll
        labelledBy='Select...'
        overrideStrings={{
          selectAll: '-- Select All --'
        }}
        onMenuToggle={handleMenuToggle}
        filterOptions={filterOptions}
      />
    </div>
  );
};

export default MultipleSelection;
