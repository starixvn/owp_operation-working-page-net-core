import PencilIcon from 'mdi-react/PencilIcon';
import PropTypes from 'prop-types';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { useStableDispatch } from '../../ImportPrematchHelperNative';
import { deleteNote } from './NoteSlice';
import { API_KEY, NOTE_TYPE, simpleApiUpdateUrl } from '../../const/const';
import { useStore } from 'react-redux';
import { updateIconNoteToPencil } from '../league/shiftSlice/shiftSlice';

const propTypes = {
  setIsEditing: PropTypes.func.isRequired,
  row: PropTypes.object.isRequired,
  model: PropTypes.object.isRequired
};

const ActionIcon = props => {
  const { setIsEditing, row, model } = props;
  const { PreMatchNoteMessageId } = row;
  const { InfoSiteMatchId, LeagueId } = model;

  const store = useStore();
  const dispatch = useStableDispatch();

  const onDelete = async () => {
    const result = await ToastrWrapper.confirm(
      'Are you sure you want to permanently delete this note ?'
    );
    if (result.value) {
      const deleteModel = {
        url: simpleApiUpdateUrl,
        apiKey: API_KEY.DELETE_NOTE,
        PreMatchNoteMessageId
      };
      await dispatch(deleteNote(deleteModel));
      const state = store.getState();
      const { notes } = state.note;
      const updateModel = InfoSiteMatchId
        ? {
            key: NOTE_TYPE.MATCH,
            value: InfoSiteMatchId
          }
        : {
            key: NOTE_TYPE.LEAGUE,
            value: LeagueId
          };

      if (!notes.length) {
        dispatch(updateIconNoteToPencil(updateModel));
      }
    }
  };

  return (
    <>
      <button className='icon-edit' onClick={() => setIsEditing(true)}>
        <PencilIcon size={12} />
      </button>
      <button className='icon-delete' onClick={onDelete} />
    </>
  );
};

ActionIcon.propTypes = propTypes;
export default ActionIcon;
