import { useEffect, useRef } from 'react';

const useClickOutside = <T extends HTMLElement>(onClickOutside: () => void) => {
  const filterRef = useRef<T>(null);
  useEffect(() => {
    const handleClickOutSide = (event: MouseEvent) => {
      if (
        filterRef.current &&
        !filterRef.current.contains(event.target as Node)
      ) {
        onClickOutside();
      }
    };
    const abort = new AbortController();
    document.addEventListener('mousedown', handleClickOutSide, abort);
    return () => {
      abort.abort();
    };
  }, [onClickOutside, filterRef]);

  return filterRef;
};

export default useClickOutside;