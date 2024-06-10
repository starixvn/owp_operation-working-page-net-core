import { createRef, useEffect } from 'react';
import PropTypes from 'prop-types';
import NoteRow from './NoteRow';
import { useSelector } from 'react-redux';
import { saveNote, clearNewCreatedNoteId } from './NoteSlice';
import { useStableDispatch } from '../../ImportPrematchHelperNative';
import { URL } from '../../const/const';
import {
  addNoteIdToLeague,
  addNoteIdToMatch
} from '../league/shiftSlice/shiftSlice';

const propTypes = {
  model: PropTypes.object.isRequired
};

const NoteTable = props => {
  const { model } = props;
  const { PreMatchNoteId, LeagueId, InfoSiteMatchId } = model;
  const addInputRef = createRef();
  const permission = useSelector(state => state.policy.permission);
  const note = useSelector(state => state.note);
  const notesData = note.notes;
  const newCreatedNoteId = note.newCreatedNoteId;
  const filterDate = useSelector(state => state.filter.filterDate);
  const dispatch = useStableDispatch();

  useEffect(() => {
    const addNoteIcon = () => {
      InfoSiteMatchId
        ? dispatch(addNoteIdToMatch({ newCreatedNoteId, InfoSiteMatchId }))
        : dispatch(addNoteIdToLeague({ newCreatedNoteId, LeagueId }));
      dispatch(clearNewCreatedNoteId());
    };
    if (newCreatedNoteId) {
      addNoteIcon();
    }
  }, [dispatch, newCreatedNoteId, InfoSiteMatchId, LeagueId]);

  const handleKeyPress = event => {
    if (event.key === 'Enter') {
      const { value } = addInputRef.current;
      let saveModel = {};
      if (!PreMatchNoteId) {
        saveModel = {
          url: URL.CREATE_NOTE,
          OWPLeagueId: LeagueId,
          InfoSiteMatchId,
          Message: value,
          EventDate: filterDate
        };
      } else {
        saveModel = {
          url: URL.INSERT_NOTE,
          PreMatchNoteId,
          Message: value
        };
      }
      value && dispatch(saveNote(saveModel));
      addInputRef.current.value = '';
    }
  };

  const renderRows = () => {
    return notesData.map((row, index) => (
      <NoteRow
        {...props}
        key={index}
        permission={permission}
        index={index}
        row={row}
      />
    ));
  };

  const renderTable = () => {
    return !notesData.length ? (
      <div className='no-data'>There is no data</div>
    ) : (
      <ul className='note-list'>{renderRows()}</ul>
    );
  };

  return (
    <div>
      {permission.HasWrite && (
        <div className='create-note'>
          <input
            className='note-textinput'
            type='text'
            placeholder='Write a note'
            ref={addInputRef}
            onKeyPress={handleKeyPress}
          />
        </div>
      )}
      {renderTable()}
    </div>
  );
};

NoteTable.propTypes = propTypes;
export default NoteTable;
