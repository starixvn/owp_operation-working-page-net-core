import Select from 'react-select';
import PropTypes from 'prop-types';

const propTypes = {
  handleChange: PropTypes.func.isRequired
};

const Year = props => {
  const date = new Date();
  const currentYear = date.getFullYear();
  const minYear = currentYear - 10;
  const years = [];

  for (let i = minYear; i <= currentYear; i++) {
    years.push(i);
  }

  const options = years.map(x => ({ value: x, label: x.toString() }));
  const defaultValue = options.filter(x => x.value === currentYear);

  return (
    <div className='filter-control'>
      <div className='label'>Year</div>
      <Select
        options={options}
        defaultValue={defaultValue}
        className='basic-multi-select'
        classNamePrefix='select'
        name='year'
        onChange={props.handleChange}
      />
    </div>
  );
};

Year.propTypes = propTypes;
export default Year;
