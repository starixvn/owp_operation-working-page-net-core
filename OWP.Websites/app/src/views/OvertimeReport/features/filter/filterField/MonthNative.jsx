import Select from 'react-select';
import PropTypes from 'prop-types';
import { months } from '../../../Const.json';

const propTypes = {
  handleChange: PropTypes.func.isRequired
};

const Month = props => {
  const date = new Date();

  return (
    <div className='filter-control'>
      <div className='label'>Month</div>
      <Select
        options={months}
        defaultValue={months[date.getMonth()]}
        className='basic-multi-select'
        classNamePrefix='select'
        name='month'
        onChange={props.handleChange}
      />
    </div>
  );
};

Month.propTypes = propTypes;
export default Month;
