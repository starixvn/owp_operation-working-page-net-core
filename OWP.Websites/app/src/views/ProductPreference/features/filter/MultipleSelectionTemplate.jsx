import MultipleSelection from '../../../Shared/MultipleSelection/MultipleSelection';
import PropTypes from 'prop-types';

const propTypes = {
  options: PropTypes.array.isRequired,
  value: PropTypes.array.isRequired,
  disabled: PropTypes.bool,
  label: PropTypes.string,
  handleChange: PropTypes.func,
  onShowDropdownListOption: PropTypes.func
};

const MultipleSelectionTemplate = props => {
  return (
    <MultipleSelection
      label={props.label}
      options={props.options}
      value={props.value}
      disabled={props.disabled}
      onChange={props.handleChange}
      onShowDropdownListOption={props.onShowDropdownListOption}
    />
  );
};

MultipleSelectionTemplate.propTypes = propTypes;
export default MultipleSelectionTemplate;
