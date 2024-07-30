import PropTypes from 'prop-types';

const propTypes = {
  onSelectionChanged: PropTypes.func.isRequired,
  selected: PropTypes.number,
  options: PropTypes.array.isRequired,
  disabled: PropTypes.bool
};

const DropdownDefaultNative = props => {
  const {
    onSelectionChanged,
    selected,
    options,
    disabled
  } = props;

  return (
    <div className='style-select'>
      <select
        data-testid="select"
        id='dropdown'
        disabled={disabled}
        className='browser-default'
        defaultValue={selected}
        onChange={onSelectionChanged}>
        {options.map(opt => {
          return (
            <option
              data-testid="select-option"
              key={opt.value}
              value={opt.value}>
              {opt.label}
            </option>
          );
        })}
      </select>
    </div>
  );
};

DropdownDefaultNative.propTypes = propTypes;

export default DropdownDefaultNative;
