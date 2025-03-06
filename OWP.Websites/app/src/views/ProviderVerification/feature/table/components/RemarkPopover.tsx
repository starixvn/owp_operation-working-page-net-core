import { useRef, useState, useEffect } from 'react';
import CloseIcon from 'mdi-react/CloseIcon';
import CommentIcon from 'mdi-react/CommentIcon';
import EditIcon from 'mdi-react/EditIcon';
import {
  UncontrolledTooltip,
  UncontrolledPopover
} from 'reactstrap';
import { TRemarkPopover } from '../../../type/table';
import { useAppDispatch } from '../../../app/hookNative';
import { saveRemark } from '../tableThunk';

const RemarkPopover = ({
  hasWrite,
  event,
  index
}: TRemarkPopover) => {
  const popupRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [remark, setRemark] = useState(event.Remark ?? '');

  const handleToggle = () => {
    setIsOpen(true);
  };

  const dispatch = useAppDispatch();


  const handleSaveRemark = () => {
    const newRemark = inputRef.current?.value.trim() || '';
    if (newRemark !== remark) {
      setRemark(newRemark);
      dispatch(saveRemark({ owpMatchId: event.OWPMatchId, remark: newRemark }));
    }
    setIsOpen(false);
  };

  useEffect(() => {
    const handleDocumentClick = (e: MouseEvent) => {
      if (popupRef.current && !popupRef.current.contains(e.target as Node)) {
        handleSaveRemark();
      }
    };

    if (isOpen) {
      document.addEventListener('click', handleDocumentClick, true);
    }

    return () => {
      document.removeEventListener('click', handleDocumentClick, true);
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

  return (
    <div className={`remark-cell${remark ? ' has-note' : ''}`}>
      {remark ? (
        <>
          <span className="icon-comment" onClick={handleToggle} id={`remark-${index}`}>
            <CommentIcon size={14} />
          </span>
          <UncontrolledTooltip target={`remark-${index}`} placement="top">
            {remark}
          </UncontrolledTooltip>
        </>
      ) : (
        <span className="icon-edit" onClick={handleToggle}>
          <EditIcon size={14} />
        </span>
      )}

      {isOpen && hasWrite && (
        <div ref={popupRef} className="note-popover is-show">
          <span className="close-popup" onClick={handleSaveRemark}>
            <CloseIcon size={14} />
          </span>

          <form className="create-note" onSubmit={handleSubmit}>
            <input
              data-testid="note-testId"
              className="note-textinput"
              type="text"
              placeholder="Write a remark"
              defaultValue={remark}
              ref={inputRef}
              onKeyDown={handleKeyDown}
            />
          </form>
        </div>
      )}
    </div>
  );
};

export default RemarkPopover;