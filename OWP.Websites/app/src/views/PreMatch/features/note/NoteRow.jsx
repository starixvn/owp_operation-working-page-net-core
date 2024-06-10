import { useState, createRef } from 'react';
import TextareaAutosize from 'react-autosize-textarea';
import dayjs from 'dayjs';
import PropTypes from 'prop-types';
import ActionIcon from './ActionIcon';
import ConfirmIcon from './ConfirmIcon';

const propTypes = {
  permission: PropTypes.object,
  row: PropTypes.object
};

const NoteRow = props => {
  const { permission, row } = props;
  const [isEditing, setIsEditing] = useState(false);
  const editNoteRef = createRef(null);
  const { Username, CreatedDate, Message } = row;

  const renderNoteReadonly = () => {
    return (
      <div className='content'>
        <div className='note-content'>
          <strong className='user-name'>@{Username}</strong> {Message}
        </div>
        <span className='date-time'>
          {CreatedDate
            ? dayjs(CreatedDate, 'YYYY-MM-DDTHH:mm:ss').format(
              'MM/DD/YYYY hh:mm A'
            )
            : ''}
        </span>
      </div>
    );
  };

  const renderNoteEditable = () => (
    <div className='content-edit'>
      <TextareaAutosize
        className='note-textinput'
        defaultValue={Message}
        ref={editNoteRef}
      />
    </div>
  );

  const renderNoteSection = () =>
    isEditing ? renderNoteEditable() : renderNoteReadonly();

  const renderConfirmIcon = () => {
    return (
      <ConfirmIcon
        setIsEditing={setIsEditing}
        editNoteRef={editNoteRef}
        {...props}
        className='confirm-icon'
      />
    );
  };

  const renderActionIcon = () => (
    <ActionIcon setIsEditing={setIsEditing} {...props} />
  );

  const renderActionSection = () => {
    return (
      <div className='icon-button'>
        {isEditing ? renderConfirmIcon() : renderActionIcon()}
      </div>
    );
  };

  return (
    <li className='note-item'>
      {renderNoteSection()}
      {permission.HasWrite && renderActionSection()}
    </li>
  );
};

NoteRow.propTypes = propTypes;
export default NoteRow;
