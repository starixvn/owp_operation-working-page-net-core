import { ColumnModels } from '../constants/preMatchConst';
import NoCell from './NoCell';
import MaxbetOUCell from './MaxbetOUCell';
import MaxbetHDPCell from './MaxbetHDPCell';
import BlacklistCell from './BlacklistCell';
import RemarkCellFactory from './RemarkCell/RemarkCellFactory';
import ActionCell from './ActionCell';
import LeagueGroupCell from './LeagueGroupCell';
import { LeagueCellFactory } from '../../../ImportPrematchHelperNative';
import AssignmentCellFactory from './AssignmentCell/AssignmentCellFactory';
import NoteCell from '../../note/NoteCell';
import IssuesCell from './IssuesCell';

const DynamicCell = (rowIndex, cellModel) => /*NOSONAR*/ {
    const { dataSource, columnKey, isEditing, handleShowNote, handleShowTicket } = cellModel;
    const {
        NoColumn,
        LeagueGroupColumn,
        LeagueColumn,
        IssuesColumn,
        MaxbetOUColumn,
        MaxbetHDPColumn,
        BlacklistColumn,
        NoteColumn,
        RemarkColumn,
        ActionColumn
    } = ColumnModels;
    switch (columnKey) {
        case NoColumn.columnKey:
            return <NoCell dataSource={dataSource} rowIndex={rowIndex} />;
        case LeagueGroupColumn.columnKey:
            return <LeagueGroupCell dataSource={dataSource} rowIndex={rowIndex} />;
        case LeagueColumn.columnKey:
            return <LeagueCellFactory dataSource={dataSource} rowIndex={rowIndex} />;
        case IssuesColumn.columnKey:
            return (
                <IssuesCell
                    dataSource={dataSource}
                    rowIndex={rowIndex}
                    handleShowTicket={handleShowTicket}
                />
            );

        case MaxbetHDPColumn.columnKey:
            return <MaxbetHDPCell dataSource={dataSource} rowIndex={rowIndex} />;
        case MaxbetOUColumn.columnKey:
            return <MaxbetOUCell dataSource={dataSource} rowIndex={rowIndex} />;
        case BlacklistColumn.columnKey:
            return <BlacklistCell dataSource={dataSource} rowIndex={rowIndex} />;
        case NoteColumn.columnKey:
            return (
                <NoteCell
                    dataSource={dataSource}
                    rowIndex={rowIndex}
                    handleShowNote={handleShowNote}
                />
            );
        case RemarkColumn.columnKey:
            return (
                <RemarkCellFactory
                    dataSource={dataSource}
                    columnId={columnKey}
                    isEditing={isEditing}
                    rowIndex={rowIndex}
                />
            );
        case ActionColumn.columnKey:
            return (
                <ActionCell
                    dataSource={dataSource}
                    isEditing={isEditing}
                    rowIndex={rowIndex}
                />
            );
        default:
            return (
                <AssignmentCellFactory
                    dataSource={dataSource}
                    rowIndex={rowIndex}
                    isEditing={isEditing}
                    columnId={columnKey}
                />
            );
    }
};

export default DynamicCell;
