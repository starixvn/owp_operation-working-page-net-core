import { Component } from 'react';
import PropTypes from 'prop-types';
import PencilIcon from 'mdi-react/PencilIcon';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
const propTypes = {
  policy: PropTypes.object.isRequired,
  disabled: PropTypes.bool.isRequired,
  isEditing: PropTypes.bool.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired
};

class ActionCell extends Component {
  render() {
    const {
      isEditing,
      onSave,
      onCancel,
      onEdit,
      policy,
      disabled
    } = this.props;
    return (
      <>
        {!policy.HasWrite && <td className='icon-button' />}
        {isEditing && (
          <td className='icon-button'>
            <button
              className='icon-save'
              disabled={disabled}
              onClick={() => onSave()}>
              <CheckIcon size={12} />
            </button>
            <button
              className='icon-cancel'
              disabled={disabled}
              onClick={() => onCancel()}>
              <CloseIcon size={12} />
            </button>
          </td>
        )}
        {!isEditing && (
          <td className='icon-button'>
            <button
              className='icon-edit'
              disabled={disabled}
              onClick={() => onEdit()}>
              <PencilIcon size={12} />
            </button>
          </td>
        )}
      </>
    );
  }
}

ActionCell.propTypes = propTypes;
export default ActionCell;
