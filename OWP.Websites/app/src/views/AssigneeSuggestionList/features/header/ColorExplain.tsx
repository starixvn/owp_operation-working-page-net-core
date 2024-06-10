import { useEffect, useRef, useState } from 'react';
import { shallow } from 'zustand/shallow';
import useStore from '../../app/useStoreNative';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
const ColorExplain = () => {
  const { colorExplainInfo, queryColorExplain } = useStore(
    state => ({
      colorExplainInfo: state.colorExplainInfo,
      queryColorExplain: state.queryColorExplain
    }),
    shallow
  );
  const [display, setDisplay] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  const renderColorExplain = () => {
    return colorExplainInfo.map(c => (
      <li
        key={c.label}
        style={{
          backgroundColor: c.backgroundColor,
          color: c.textColor
        }}>
        {c.label}
      </li>
    ));
  };

  const showHideColorExplain = () => {
    !display && !colorExplainInfo.length && queryColorExplain();
    setDisplay(!display);
  };

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        display &&
        wrapperRef.current &&
        !wrapperRef.current.contains(event.target as Node)
      ) {
        setDisplay(!display);
      }
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [wrapperRef, display]);

  return (
    <div
      className='color__explain_info'
      ref={wrapperRef}
      onClick={showHideColorExplain}>
      <span className='info'>
        <InfoOutlinedIcon />
      </span>
      <ul>{display && renderColorExplain()}</ul>
    </div>
  );
};

export default ColorExplain;
