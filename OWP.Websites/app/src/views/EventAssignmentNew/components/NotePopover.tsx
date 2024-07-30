import { useRef, useState, useEffect } from 'react';
import CloseIcon from 'mdi-react/CloseIcon';
import CommentIcon from 'mdi-react/CommentIcon';
import { EditIcon } from 'mdi-react';
import { TDataSource, TNote } from '../type/type';
import NoteTable from '../../LiveDetail/MatchNote/NoteTable';

interface NotePopoverProps {
  policy: { [key: string]: boolean };
  event: TDataSource;
  onShowNote: (show: boolean) => void;
}

const NotePopover = ({
  policy,
  event,
  onShowNote
}: NotePopoverProps) => {
  const popupRef = useRef<HTMLDivElement>(null);
  const [isOpen, setIsOpen] = useState(false);
  const handleToggle = () => {
    setIsOpen(true);
    onShowNote(true);
  };

  const handleSave = (notes: TNote[]) => {
    event.Notes = notes;
  };

  useEffect(() => {
    const handleDocumentClick = (e: MouseEvent) => {
      if (popupRef.current && !popupRef.current.contains(e.target as Node)) {
        setIsOpen(false);
      }
    };

    if (isOpen) {
      document.addEventListener('click', handleDocumentClick, true);
    } else {
      document.removeEventListener('click', handleDocumentClick, true);
    }

    return () => {
      document.removeEventListener('click', handleDocumentClick, true);
    };
  }, [isOpen]);

  useEffect(() => {
    isOpen && onShowNote && onShowNote(isOpen);
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpen]);

  const hasNote = event.Notes.length > 0;

  return (
    <div className={`note-cell${hasNote ? ' has-note' : ''}`}>
      {hasNote ? (
        <span className='icon-comment' onClick={handleToggle}>
          <CommentIcon size={14} />
        </span>
      ) : (
        <span className='icon-edit' onClick={handleToggle}>
          <EditIcon size={14} />
        </span>
      )}
      {isOpen && (
        <div ref={popupRef} className={`note-popover is-show`}>
          <span className='close-popup' onClick={() => setIsOpen(false)}>
            <CloseIcon size={14} />
          </span>
          <NoteTable
            matchId={event.OWPMatchId}
            policy={policy}
            rows={event.Notes}
            onSave={handleSave}
          />
        </div>
      )}
    </div>
  );
};

export default NotePopover;
