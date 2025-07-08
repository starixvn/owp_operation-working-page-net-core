import { useEffect, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { REQUEST_STATUS } from '../../Shared/Constants';
import Loading from '../../components/Loading';
import { Table } from 'fixed-data-table-2';
import { TABLE } from '../const.json';
import renderColumn from './column/renderColumn';
import store from '../app/store';
import { setAllowAddEvent } from './allowAddEventSlice';
import NotePopupTableCss from '../../Shared/NotePopupTableCss';
import { withResizeDetector } from 'react-resize-detector';
import ReactDOM from 'react-dom';

const {
  ROW_HEIGHT,
  HEADER_HEIGHT,
  MAX_HEIGHT
} = TABLE;

const EventAssignmentTable = () => {
  const tableRef = useRef(null);
  const dispatch = useAppDispatch();
  const { entities, loading, matchesSelected } = useAppSelector(state => state.eventAssignmentTable);
  const {
    filter: {
      filterState: { selectedDate }
    }
  } = store.getState();


  useEffect(() => {
    dispatch(setAllowAddEvent(selectedDate));
  }, [dispatch, selectedDate]);

  if (loading === REQUEST_STATUS.pending) {
    return <Loading />;
  }
  const tableWith =
    window.innerWidth > 680 ? window.innerWidth - 125 : window.innerWidth;

  const handleShowPopup = (show: boolean) => {
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react/no-find-dom-node
    const tableElement = ReactDOM.findDOMNode(tableRef.current);
    NotePopupTableCss(tableElement, show);
  };

  return (
    <div className='table'>
      <Table
        ref={tableRef}
        rowHeight={ROW_HEIGHT}
        rowsCount={entities.length}
        width={tableWith}
        height={window.innerHeight - 170}
        maxHeight={MAX_HEIGHT}
        headerHeight={HEADER_HEIGHT}>
        {renderColumn(entities, handleShowPopup, matchesSelected)}
      </Table>
      <div className='total'>Total records:{entities.length}</div>
    </div>
  );
};

export default withResizeDetector(EventAssignmentTable);
