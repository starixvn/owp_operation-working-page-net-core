import {
  Children,
  isValidElement,
  cloneElement,
  useState,
  useLayoutEffect,
  useCallback,
  useEffect,
  useRef
} from 'react';
import { throttle } from 'lodash';
import PropTypes from 'prop-types';

const propTypes = {
  data: PropTypes.array,
  tableHeader: PropTypes.element,
  classNameTable: PropTypes.string,
  itemRowHeight: PropTypes.number,
  children: PropTypes.element
};

const CustomTable = ({
  data,
  tableHeader,
  classNameTable,
  itemRowHeight,
  children
}) => {
  const containerRef = useRef(null);
  const [maxHeight, setMaxHeight] = useState(window.innerHeight - 200);
  const [maxWidth, setMaxWidth] = useState(window.innerWidth);
  const [displayStart, setDisplayStart] = useState(0);
  const [displayEnd, setDisplayEnd] = useState(0);
  const [scrollPosition, setScrollPosition] = useState(0);
  const screenHeight = maxHeight;
  const offset = screenHeight;
  const rowsToRender = 30;

  const setDisplayPositions = useCallback(
    scroll => {
      const scrollWithOffset = Math.floor(scroll - rowsToRender - offset / 2);
      const displayStartPosition = Math.round(
        Math.max(0, Math.floor(scrollWithOffset / itemRowHeight))
      );
      const displayEndPosition = Math.round(
        Math.min(displayStartPosition + rowsToRender, data.length)
      );
      if (displayStartPosition + rowsToRender - 1 <= displayEndPosition) {
        setDisplayStart(displayStartPosition);
      } else if (displayStartPosition > displayEndPosition) {
        setDisplayStart(0);
      } else {
        setDisplayStart(previous => previous);
      }
      setDisplayEnd(displayEndPosition);
    },
    [data.length]
  );

  const onScrollTable = throttle(() => {
    const scrollTop = containerRef.current?.scrollTop;
    if (data.length !== 0) {
      setScrollPosition(scrollTop);
      setDisplayPositions(scrollTop);
    }
  }, 100);

  const handleResizeBrowser = () => {
    const { innerHeight, innerWidth } = window;
    setMaxHeight(innerHeight - 200);
    setMaxWidth(innerWidth);
  };

  const childrenWithProps = Children.map(children, child => {
    if (isValidElement(child)) {
      return cloneElement(child, {
        data,
        displayStart,
        displayEnd
      });
    }
    return child;
  });

  useLayoutEffect(() => {
    window.addEventListener('resize', handleResizeBrowser);
    return () => window.removeEventListener('resize', handleResizeBrowser);
  }, []);

  useEffect(() => {
    setDisplayPositions(scrollPosition);
  }, [scrollPosition, setDisplayPositions]);

  useEffect(() => {
    if (data.length === 0) {
      setDisplayStart(0);
      setDisplayEnd(0);
      setScrollPosition(0);
    }
    const scrollTo=containerRef.current?.scrollTo;
    scrollTo && scrollTo(0, 0);
  }, [data.length]);

  return (
    <>
      <div
        ref={containerRef}
        onScroll={onScrollTable}
        className='table-wrap'
        style={{ maxHeight, maxWidth }}>
        <table id='scrollable-table' className={classNameTable}>
          <thead>{tableHeader}</thead>
          <tbody>{childrenWithProps}</tbody>
        </table>
      </div>
      <div>Total record: {data.length}</div>
    </>
  );
};

CustomTable.propTypes = propTypes;
export default CustomTable;
