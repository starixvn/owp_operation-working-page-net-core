import { useState } from 'react';
import HistoryPopup from './HistoryPopup';

type Props = {
  userName: string;
  userId: number;
};

const HistoryCell = ({ userId, userName }: Props) => {
  const [isOpen, setIsOpen] = useState(false);
  const handleClosePopup = () => setIsOpen(false);
  return (
    <>
      <div>
        <span
          className='icon-history'
          id={`history-icon-${userId}`}
          onClick={() => setIsOpen(!isOpen)}></span>
      </div>
      {isOpen && (
        <HistoryPopup
          handleClosePopup={handleClosePopup}
          userId={userId}
          userName={userName}
        />
      )}
    </>
  );
};

export default HistoryCell;
