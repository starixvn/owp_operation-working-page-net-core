import { useLayoutEffect, useState } from 'react';

const useLayoutTable = () => {
  const [maxHeight, setMaxHeight] = useState(window.innerHeight - 200);
  const [maxWidth, setMaxWidth] = useState(window.innerWidth);

  const handleResizeBrowser = () => {
    const { innerHeight, innerWidth } = window;
    setMaxHeight(innerHeight - 200);
    setMaxWidth(innerWidth);
  };

  useLayoutEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  return { maxHeight, maxWidth };
};

export default useLayoutTable;
