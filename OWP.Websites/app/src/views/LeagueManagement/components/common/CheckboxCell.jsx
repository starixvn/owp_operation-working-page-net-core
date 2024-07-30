import PropTypes from 'prop-types';
import { Input } from 'reactstrap';
import { Cell } from 'fixed-data-table-2';
import { v4 as uuidv4 } from 'uuid';

const propTypes = {
  defaultCheck: PropTypes.bool,
  onChangeChecked: PropTypes.func.isRequired,
  id: PropTypes.string.isRequired,
  hasWrite: PropTypes.bool
};

const CheckboxCell = ({ defaultCheck, onChangeChecked, id, hasWrite }) => {
  const handleChange = event => {
    const { checked } = event.target;
    onChangeChecked(checked);
  };
  return (
    <Cell key={uuidv4()} className='chb-active-blacklist checkbox form-check'>
      <Input
        disabled={!hasWrite}
        id={id}
        defaultChecked={defaultCheck}
        type='checkbox'
        className='form-check-input'
        onChange={handleChange}
      />
      <label className='form-check-label' htmlFor={id} />
    </Cell>
  );
};

CheckboxCell.propTypes = propTypes;
export default CheckboxCell;
