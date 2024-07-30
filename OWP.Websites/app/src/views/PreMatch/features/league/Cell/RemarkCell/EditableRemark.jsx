import { createRef } from 'react';
import PropTypes from 'prop-types';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import { updatePrematch } from '../../shiftSlice/shiftSlice';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired,
  columnId: PropTypes.string.isRequired
};

const EditableRemark = props => {
  const { rowIndex, dataSource, columnId } = props;
  const { Remark } = dataSource[rowIndex];
  const stableDispatch = useStableDispatch();
  const input = createRef();

  const handleUpdateRemark = () => {
    stableDispatch(
      updatePrematch({
        rowIndex,
        newValue: input.current.value,
        key: columnId
      })
    );
  };

  return (
    <input
      ref={input}
      type='text'
      className='input-field'
      defaultValue={Remark}
      maxLength='200'
      onBlur={handleUpdateRemark}
    />
  );
};

EditableRemark.propTypes = propTypes;
export default EditableRemark;
