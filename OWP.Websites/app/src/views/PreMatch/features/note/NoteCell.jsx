import { createRef, useEffect } from 'react';
import PropTypes from 'prop-types';
import CommentIcon from 'mdi-react/CommentIcon';
import { EditIcon } from 'mdi-react';
import NotePopover from './NotePopover';
import { useStableDispatch } from '../../ImportPrematchHelperNative';
import {
  changeStatePopupMatchNote,
  changeStatePopupLeagueNote
} from '../league/shiftSlice/shiftSlice';
import { clearNote, getNote } from './NoteSlice';
import { API_KEY, simpleApiQueryUrl } from '../../const/const';
import { useSelector } from 'react-redux';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  handleShowNote: PropTypes.func.isRequired
};

const NoteCell = props => {
  const { dataSource, rowIndex, handleShowNote } = props;
  const model = dataSource[rowIndex];
  const {
    HasNotes,
    isOpenNote,
    PreMatchLeagueId,
    InfoSiteMatchId,
    PreMatchNoteId
  } = model;
  const iconRef = createRef();
  const stableDispatch = useStableDispatch();
  const permission = useSelector(state => state.policy.permission);

  useEffect(() => {
    handleShowNote();
  }, [handleShowNote, isOpenNote]);

  const renderCommentIcon = () => <CommentIcon size={14} />;

  const renderEditIcon = () => <EditIcon size={14} />;

  const classNameIconSelection = HasNotes ? 'icon-comment' : 'icon-edit';
  const classNameNote = `note-cell ${HasNotes ? ' has-note' : ''}`;

  const handleClick = value => {
    InfoSiteMatchId
      ? stableDispatch(
          changeStatePopupMatchNote({ InfoSiteMatchId, openState: value })
        )
      : stableDispatch(
          changeStatePopupLeagueNote({ PreMatchLeagueId, openState: value })
        );
    stableDispatch(clearNote());
    const getModel = {
      url: simpleApiQueryUrl,
      apiKey: API_KEY.QUERY_NOTE,
      PreMatchNoteId
    };
    stableDispatch(getNote(getModel));
  };

  return (
    <div className={classNameNote}>
      <span
        ref={iconRef}
        className={classNameIconSelection}
        onClick={() => handleClick(true)}>
        {HasNotes
          ? renderCommentIcon()
          : permission.HasWrite && renderEditIcon()}
      </span>
      {isOpenNote && <NotePopover setIsOpening={handleClick} model={model} />}
    </div>
  );
};

NoteCell.propTypes = propTypes;
export default NoteCell;
