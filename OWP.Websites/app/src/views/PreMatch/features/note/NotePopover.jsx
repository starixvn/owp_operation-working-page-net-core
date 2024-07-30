import PropTypes from 'prop-types';
import CloseIcon from 'mdi-react/CloseIcon';
import NoteTable from './NoteTable';

const propTypes = {
  setIsOpening: PropTypes.func.isRequired
};

const NotePopover = props => {
  const { setIsOpening } = props;

  return (
    <div className='note-popover is-show'>
      <span className='close-popup' onClick={() => setIsOpening(false)}>
        <CloseIcon size={14} />
      </span>
      <NoteTable {...props} />
    </div>
  );
};

NotePopover.propTypes = propTypes;
export default NotePopover;
