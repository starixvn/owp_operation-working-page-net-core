import createColumn from './createColumn';
import { ColumnModels } from './constants/preMatchConst';

const ColumnsFactory = (dataSource, props, columnWidth) => {
    const {
        NoColumn,
        LeagueGroupColumn,
        LeagueColumn,
        HDPColumn,
        OUColumn,
        HDPTakeOverColumn,
        OUTakeOverColumn,
        MaxbetOUColumn,
        MaxbetHDPColumn,
        BlacklistColumn,
        NoteColumn,
        RemarkColumn,
        ActionColumn,
        IssuesColumn,
        BettypeHdpColumn,
        BettypeHdpOverColumn,
        BettypeOuColumn,
        BettypeOuOverColumn
    } = ColumnModels;

    const {
        sortState,
        isEditing,
        handleShowNote,
        handleShowTicket,
        isRedirect,
        isNightShift
    } = props;

    const SortableColumn = {
        sortState,
        isEditing,
        isCanSort: true,
        isNightShift
    };

    const column = {
        sortState,
        isEditing,
        isCanSort: false,
        handleShowNote,
        handleShowTicket
    };

    const columns = [];

    const columnSchemaStaticInfo = [
        { ...LeagueGroupColumn, width: columnWidth.LeagueGroup, ...SortableColumn },
        { ...LeagueColumn, width: columnWidth.League, ...SortableColumn },
        { ...IssuesColumn, width: columnWidth.Issues, ...column },
        {
            ...HDPColumn,
            width: columnWidth.HDP,
            ...SortableColumn,
            ...BettypeHdpColumn,
            isRedirect
        },
        {
            ...OUColumn,
            width: columnWidth.OU,
            ...SortableColumn,
            ...BettypeOuColumn,
            isRedirect
        },
        {
            ...HDPTakeOverColumn,
            width: columnWidth.HDPTakeOver,
            ...SortableColumn,
            ...BettypeHdpOverColumn,
            isRedirect
        },
        {
            ...OUTakeOverColumn,
            width: columnWidth.OUTakeOver,
            ...SortableColumn,
            ...BettypeOuOverColumn,
            isRedirect
        },
        { ...MaxbetHDPColumn, width: columnWidth.MaxBetHDP },
        { ...MaxbetOUColumn, width: columnWidth.MaxBetOU },
        {
            ...BlacklistColumn,
            width: columnWidth.BlackList,
            ...SortableColumn
        },
        { ...NoteColumn, width: columnWidth.Note, ...column },
        { ...RemarkColumn, width: columnWidth.Remark, ...column }
    ];

    isEditing
        ? columnSchemaStaticInfo.unshift({
            ...ActionColumn,
            width: columnWidth.Action,
            ...column
        })
        : columnSchemaStaticInfo.unshift({
            ...NoColumn,
            width: columnWidth.No,
            ...SortableColumn
        });

    columnSchemaStaticInfo.forEach(col =>
        columns.push(createColumn(dataSource, col))
    );

    return columns;
};

export default ColumnsFactory;
