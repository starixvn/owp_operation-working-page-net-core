import Select from 'react-select';
import PropTypes from 'prop-types';

const propTypes = {
  option: PropTypes.array.isRequired,
  onSelectedChange: PropTypes.func.isRequired
};

const SelectFilterFactory = props => {
  const { option, onSelectedChange } = props;
  return (
    <Select
      data-testId='select'
      options={option}
      defaultValue={option[0]}
      onChange={onSelectedChange}
      className='basic-filter-select'
    />
  );
};

SelectFilterFactory.propTypes = propTypes;
export default SelectFilterFactory;
