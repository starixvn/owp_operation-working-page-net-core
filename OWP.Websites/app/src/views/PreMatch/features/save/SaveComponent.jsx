import PropTypes from 'prop-types';

const propType = {
  onCancel: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired
};

const SaveComponent = ({ onSave, onCancel }) => {
  return (
    <div className='button-cancel-save'>
      <button className='button-cancel' onClick={onCancel}>
        Cancel
      </button>
      <button className='button-submit' onClick={onSave}>
        Save
      </button>
    </div>
  );
};

SaveComponent.propTypes = propType;

export default SaveComponent;
