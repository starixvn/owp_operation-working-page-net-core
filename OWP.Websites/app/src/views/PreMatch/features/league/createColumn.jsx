import { Column, Cell } from 'fixed-data-table-2';
import DynamicCell from './Cell/DynamicCell';
import OrderHeader from './OrderHeader';

const renderHeaderCell = headerModel => {
    return headerModel.isCanSort ? (
        <OrderHeader {...headerModel} />
    ) : (
        headerModel.headerName
    );
};

const createColumn = (dataSource, headerInfo) => {
    const {
        columnKey,
        headerName,
        width,
        color,
        sortState,
        isEditing,
        handleShowNote,
        handleShowTicket,
        isCanSort = false,
        fixed,
        isRedirect,
        bettypeId,
        isTakeOver,
        isNightShift
    } = headerInfo;

    const headerModel = {
        headerName,
        columnKey,
        sortState,
        isCanSort,
        isRedirect,
        bettypeId,
        isTakeOver,
        isNightShift
    };

    const cellModel = {
        dataSource,
        columnKey,
        isEditing,
        handleShowNote,
        handleShowTicket
    };

    return (
        <Column
            key={headerName}
            columnKey={columnKey}
            fixed={fixed}
            header={
                <Cell className={`header ${color}`}>
                    {renderHeaderCell(headerModel)}
                </Cell>
            }
            cell={({ rowIndex }) => DynamicCell(rowIndex, cellModel)}
            width={width}
        />
    );
};

export { renderHeaderCell };
export default createColumn;
