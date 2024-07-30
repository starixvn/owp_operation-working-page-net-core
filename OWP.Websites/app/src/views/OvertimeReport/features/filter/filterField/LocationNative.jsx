import { useSelector } from 'react-redux';
import Select from 'react-select';
import PropTypes from 'prop-types';

const propTypes = {
  handleChange: PropTypes.func.isRequired,
  selectedLocation: PropTypes.object.isRequired
};

const Location = props => {
  const store = useSelector(state => state.filter);
  
  return (
    <div className='filter-control'>
      <div className='label'>Location</div>
      <Select
        options={store.locations}
        defaultValue={props.selectedLocation}
        className='basic-multi-select'
        classNamePrefix='select'
        name='location'
        onChange={props.handleChange}
      />
    </div>
  );
};

Location.propTypes = propTypes;
export default Location;
