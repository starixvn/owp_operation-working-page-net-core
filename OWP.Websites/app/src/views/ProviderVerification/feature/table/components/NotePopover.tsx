import { useRef, useState, useEffect } from 'react';
import CloseIcon from 'mdi-react/CloseIcon';
import CommentIcon from 'mdi-react/CommentIcon';
import EditIcon from 'mdi-react/EditIcon';
import { TRemarkPopover } from '../../../type/table';
import { useAppDispatch } from '../../../app/hookNative';
import { saveRemark, saveRemarkBy } from '../tableThunk';
import { UncontrolledTooltip } from 'reactstrap';

const NotePopover = ({
  className,
  hasWrite,
  OWPMatchId,
  value,
  index,
  isRemark,
  canEditDate
}: TRemarkPopover) => {
  const popupRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [note, setNote] = useState(value ?? '');
  const dispatch = useAppDispatch();

  const handleOpen = () => setIsOpen(true);

  const handleSaveRemark = () => {
    const newNote = inputRef.current?.value.trim() || '';
    if (newNote !== note) {
      setNote(newNote);
      isRemark
        ? dispatch(saveRemark({ owpMatchId: OWPMatchId, remark: newNote }))
        : dispatch(saveRemarkBy({ owpMatchId: OWPMatchId, remarkBy: newNote }));
    }
    setIsOpen(false);
  };

  useEffect(() => {
    if (!isOpen) {
      return undefined;
    }

    const handleClickOutside = (e: MouseEvent) => {
      if (popupRef.current && !popupRef.current.contains(e.target as Node)) {
        handleSaveRemark();
      }
    };

    document.addEventListener('click', handleClickOutside, true);
    return () => {
      document.removeEventListener('click', handleClickOutside, true);
    };
  }, [isOpen]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    handleSaveRemark();
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleSaveRemark();
    }
  };

  const placeholderText = isRemark ? 'Write a remark' : 'Write a remark by';
  const cellId = `${className}-${index}`;

  return (
    <div className={className} id={cellId}>
      <div className={`note-cell${note ? ' has-note' : ''}`}>
        {note ? (
          <>
            <span
              className='icon-comment'
              {...(canEditDate ? { onClick: handleOpen } : {})}
              id={`note-${index}`}>
              <CommentIcon size={14} />
            </span>
            <UncontrolledTooltip target={cellId} placement='top'>
              {note}
            </UncontrolledTooltip>
          </>
        ) : (
          canEditDate && (
            <span className='icon-edit' onClick={handleOpen}>
              <EditIcon size={14} />
            </span>
          )
        )}

        {isOpen && hasWrite && (
          <div ref={popupRef} className='note-popover is-show'>
            <span className='close-popup' onClick={handleSaveRemark}>
              <CloseIcon size={14} />
            </span>

            <form className='create-note' onSubmit={handleSubmit}>
              <input
                data-testid='note-testId'
                className='note-textinput'
                type='text'
                placeholder={placeholderText}
                defaultValue={note}
                ref={inputRef}
                onKeyDown={handleKeyDown}
              />
            </form>
          </div>
        )}
      </div>
    </div>
  );
};

export default NotePopover;
