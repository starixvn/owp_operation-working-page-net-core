import { useState, useEffect } from 'react';

const useAssignToMe = (key, me, empFilterRef) => {
  const isCached = Boolean(localStorage.getItem(key));
  const [isDisable, setIsDisable] = useState(isCached);
  const cacheLocalStore = () =>
    isDisable ? localStorage.setItem(key, me) : localStorage.removeItem(key);

  useEffect(() => {
    if (isCached && empFilterRef.current) {
      empFilterRef.current.value = me;
    }
  }, [empFilterRef, isCached, me]);

  const handleOnClick = () => {
    if (isDisable) {
      empFilterRef.current.value = '';
      setIsDisable(false);
    } else {
      empFilterRef.current.value = me;
      setIsDisable(true);
    }
  };

  return [handleOnClick, isDisable, cacheLocalStore];
};

export default useAssignToMe;
