import Select from 'react-select';
import PropTypes from 'prop-types';

const propTypes = {
  option: PropTypes.array.isRequired,
  onSelectedChange: PropTypes.func.isRequired
};

const SingleSelection = props => {
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

SingleSelection.propTypes = propTypes;
export default SingleSelection;
