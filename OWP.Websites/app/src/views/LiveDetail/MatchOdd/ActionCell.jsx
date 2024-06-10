import { Component } from 'react';
import PropTypes from 'prop-types';
import PencilIcon from 'mdi-react/PencilIcon';
import DeleteIcon from 'mdi-react/DeleteIcon';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
const propTypes = {
  disabled: PropTypes.bool.isRequired,
  isEditing: PropTypes.bool.isRequired,
  isMultipleEdit: PropTypes.bool,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired
};

class ActionCell extends Component {
  constructor(props) {
    super(props);
    this.state = {
      modal: false
    };
  }

  toggle() {
    this.setState(prevState => ({
      modal: !prevState.modal
    }));
  }

  render() {
    const {
      isEditing,
      onSave,
      onCancel,
      onEdit,
      onDelete,
      disabled,
      isMultipleEdit
    } = this.props;

    if (isMultipleEdit) {
      return (
        <td className='icon-button text-center'>
          <button
            className='icon-delete'
            disabled={disabled}
            onClick={() => onDelete()}>
            <DeleteIcon size={12} />
          </button>
        </td>
      );
    } else {
      if (isEditing) {
        return (
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
        );
      } else {
        return (
          <td className='icon-button'>
            <button
              className='icon-edit'
              disabled={disabled}
              onClick={() => onEdit()}>
              <PencilIcon size={12} />
            </button>
            <button
              className='icon-delete'
              disabled={disabled}
              onClick={() => onDelete()}>
              <DeleteIcon size={12} />
            </button>
          </td>
        );
      }
    }
  }
}

ActionCell.propTypes = propTypes;
export default ActionCell;
