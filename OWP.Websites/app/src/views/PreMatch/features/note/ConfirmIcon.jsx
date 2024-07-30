import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import PropTypes from 'prop-types';
import { useStableDispatch } from '../../ImportPrematchHelperNative';
import { URL } from '../../const/const';
import { updateNote } from './NoteSlice';

const propTypes = {
  setIsEditing: PropTypes.func.isRequired,
  row: PropTypes.object.isRequired,
  editNoteRef: PropTypes.object.isRequired
};

const ConfirmIcon = props => {
  const { setIsEditing, row, editNoteRef } = props;
  const { PreMatchNoteMessageId } = row;
  const dispatch = useStableDispatch();

  const onUpdate = () => {
    const postModel = {
      PreMatchNoteMessageId,
      Message: editNoteRef.current.value,
      url: URL.UPDATE_NOTE
    };
    dispatch(updateNote(postModel));
    setIsEditing(false);
  };

  return (
    <>
      <button className='icon-save' onClick={onUpdate}>
        <CheckIcon size={12} />
      </button>
      <button className='icon-cancel' onClick={() => setIsEditing(false)}>
        <CloseIcon size={12} />
      </button>
    </>
  );
};

ConfirmIcon.propTypes = propTypes;
export default ConfirmIcon;
