import { useEffect } from 'react';
import { useAppSelector } from './app/hooks';

const BlinkDocumentTitle = () => {
  const isBlinkTitle = useAppSelector(state => state.chatBoard.isBlinkTitle);
  const originalTitle = 'Operation Working Page';
  useEffect(() => {
    const toggleTitle = () => {
      document.title =
        originalTitle === document.title ? 'New message' : originalTitle;
    };
    const intervalId = isBlinkTitle ? setInterval(toggleTitle, 1000) : null;
    return () => {
      clearInterval(intervalId);
      document.title = originalTitle;
    };
  }, [isBlinkTitle]);

  return null;
};

export default BlinkDocumentTitle;
