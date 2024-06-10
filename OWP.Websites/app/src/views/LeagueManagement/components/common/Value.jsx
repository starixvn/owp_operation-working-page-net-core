import { createRef } from 'react';
import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import { useDispatch } from 'react-redux';
import { crud } from '../../actions/action';

const propTypes = {
  item: PropTypes.object.isRequired,
  index: PropTypes.number.isRequired,
  hasWrite: PropTypes.bool,
  actionType: PropTypes.object.isRequired
};
const Value = props => {
  const { item, index, hasWrite, actionType } = props;
  const dispatch = useDispatch();
  let inputRef = createRef();
  const handleOnBlur = event => {
    event.preventDefault();
    dispatch(
      crud(actionType.UPDATE, {
        key: 'value',
        value: inputRef.value,
        index
      })
    );
  };

  const handleOnChange = input => (inputRef = input);

  return (
    <Input
      disabled={!hasWrite}
      type='text'
      name='league-value'
      className='input-field'
      defaultValue={item.value}
      maxLength='100'
      onBlur={handleOnBlur}
      innerRef={handleOnChange}
    />
  );
};

Value.propTypes = propTypes;

export default Value;
