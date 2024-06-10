import { Component } from 'react';
import PropTypes from 'prop-types';
import PencilIcon from 'mdi-react/PencilIcon';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';

const propTypes = {
  onEdit: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  isEditing: PropTypes.bool.isRequired
};

class EditableButtonGroup extends Component {
  render = () => {
    const { onEdit, onSave, onCancel, isEditing } = this.props;
    return (
      <div className='btn-action'>
        {!isEditing && (
          <div className='edit'>
            <button
              data-testid="edit-testId"
              className='action icon-edit'
              onClick={onEdit}>
              <PencilIcon size={13} />
            </button>
          </div>
        )}
        {isEditing && (
          <div className='save'>
            <button className='action icon-save' onClick={onSave}>
              <CheckIcon size={14} />
            </button>
            <button className='action icon-cancel' onClick={onCancel}>
              <CloseIcon size={14} />
            </button>
          </div>
        )}
      </div>
    );
  };
}

EditableButtonGroup.propTypes = propTypes;
export default EditableButtonGroup;
