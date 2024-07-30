import { createRef } from 'react';
import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import { useDispatch } from 'react-redux';
import { crud } from '../../../actions/action';

const propTypes = {
  hasTV: PropTypes.bool.isRequired,
  tv: PropTypes.string,
  actionType: PropTypes.object.isRequired
};
const Tv = ({ hasTV, tv, actionType }) => {
  let inputRef = createRef();
  const id = 'tV';
  const dispatch = useDispatch();
  const triggerChangeEvent = (key, value) => {
    dispatch(
      crud(actionType.UPDATE, {
        key,
        value
      })
    );
  };
  const handleHasTVChange = event => {
    const { checked } = event.target;
    triggerChangeEvent('hasTV', checked);
  };
  const handleOnBlur = () => {
    triggerChangeEvent('tv', inputRef.value);
  };

  const handleValueChange = input => (inputRef = input);

  return (
    <div className='list has-tv'>
      <div className='list-title'>Has TV</div>
      <div className='list-contain'>
        <div className='list-item'>
          <div className='checkbox'>
            <Input
              id={id}
              defaultChecked={hasTV}
              type='checkbox'
              className='form-check-input'
              onChange={handleHasTVChange}
            />
            <label className='form-check-label' htmlFor={id}>
              Has TV
            </label>
          </div>
          <Input
            type='text'
            name='league-value'
            className='input-field'
            defaultValue={tv}
            maxLength='100'
            onBlur={handleOnBlur}
            innerRef={handleValueChange}
          />
        </div>
      </div>
    </div>
  );
};

Tv.propTypes = propTypes;
export default Tv;
