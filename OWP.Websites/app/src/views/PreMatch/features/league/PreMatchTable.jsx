import { createRef, useEffect, useMemo } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import { DefaultTableStyle } from './constants/preMatchConst';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import { Table } from 'fixed-data-table-2';
import ColumnsFactory from './ColumnsFactory';
import ReactDOM from 'react-dom';
import NotePopupTableCss from '../../../Shared/NotePopupTableCss';
import {
    calculateColumnWidth,
    calculateRowHeight,
    calculateTableWidth
} from './PrematchTableHelper';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { resetCopyInfo } from '../copy/copySlice';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getMMDDYYYYStringFromDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import { withResizeDetector } from 'react-resize-detector';
import { getTotalLeagues } from '../../features/league/shiftSlice/shiftHelper';

const { rowHeight, headerHeight } = DefaultTableStyle;

const propTypes = {
    isEditing: PropTypes.bool.isRequired,
    isNightShift: PropTypes.bool
};

const PreMatchTable = ({ isEditing, isNightShift }) => {
    const model = useSelector(state => state.dayShift);
    const filter = useSelector(state => state.filter);
    const tableKey = model.tableKey;
    const data = model.entities;
    const dataLength = data.length;
    const tableRef = createRef();
    const dispatch = useStableDispatch();

    useEffect(() => {
        const handleEscKey = event => {
            if (isEqual(event.keyCode, 27)) {
                dispatch(resetCopyInfo());
            }
        };
        document.addEventListener('keydown', handleEscKey);
        return () => document.removeEventListener('keydown', handleEscKey);
    }, [dispatch]);

    const handleShowNote = () => {
        // eslint-disable-next-line no-warning-comments
        // eslint-disable-next-line react/no-find-dom-node
        const tableElement = ReactDOM.findDOMNode(tableRef.current);
        NotePopupTableCss(tableElement, true);
    };

    const submitIsToday =
        getMMDDYYYYStringFromDateTime(filter.filterDate) ===
        getMMDDYYYYStringFromDateTime(new Date());

    const newProps = {
        isEditing,
        sortState: model.sortState,
        handleShowNote,
        handleShowTicket: handleShowNote,
        isRedirect: submitIsToday,
        isNightShift
    };

    const getRowHeight = index => calculateRowHeight(data[index], isEditing);

    const rowClassNameGetter = index => {
        const classNames = [];
        dataLength > 7 && index > dataLength - 6 && classNames.push('is-last-row');

        return classNames.join(' ');
    };
    const width = window.innerWidth > 680 ? window.innerWidth - 110 : window.innerWidth;
    const columnWidth = useMemo(
        () => calculateColumnWidth(isEditing, width),
        [isEditing, width]
    );

    const tableWidth = width > 1700 ? calculateTableWidth(columnWidth) : width;

    const tableHeight = window.innerHeight - 200;

    if (isEqual(model.loading, REQUEST_STATUS.pending)) {
        return <Loading />;
    }

    return (
        <div className='table'>
            <Table
                ref={tableRef}
                rowHeight={rowHeight}
                rowsCount={dataLength}
                key={tableKey}
                rowHeightGetter={getRowHeight}
                width={tableWidth}
                height={tableHeight}
                maxHeight={tableHeight}
                rowClassNameGetter={rowClassNameGetter}
                headerHeight={headerHeight}>
                {ColumnsFactory(data, newProps, columnWidth)}
            </Table>
            <div className='total'>Total Leagues: {getTotalLeagues(data)}</div>
        </div>
    );
};

PreMatchTable.propTypes = propTypes;
PreMatchTable.displayName = 'PreMatchTable';
export default withResizeDetector(PreMatchTable);
