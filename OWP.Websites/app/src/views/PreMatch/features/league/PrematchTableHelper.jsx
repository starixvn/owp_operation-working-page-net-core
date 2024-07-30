import { DefaultTableStyle, ColumnModels } from './constants/preMatchConst';

const calculateRowLevel = (
  inputString,
  numberCharacterPerLevel,
  initialLevel = 0
) =>
  inputString
    ? Math.ceil(inputString.length / numberCharacterPerLevel) + initialLevel
    : 1;

const mapRowLevelToHight = rowLevel => {
  switch (rowLevel) {
    case 0:
    case 1:
      return 38;
    case 2:
      return 68;
    case 3:
      return 90;
    default:
      return 108;
  }
};

const selectOtherBettype = (isEdited, source1, source2) =>
  !isEdited ? source1 : source1 || source2;

const calculateRowHeight = (row, isEdited) => {
  const hights = [];
  if (row) {
    const otherHdpTarget = selectOtherBettype(
      isEdited,
      row.OtherHDP,
      row.OtherHDPHint
    );
    const otherOuTarget = selectOtherBettype(
      isEdited,
      row.OtherOU,
      row.OtherOUHint
    );
    const otherHdpTakeOverTarget = selectOtherBettype(
      isEdited,
      row.OtherHDPTakeOver,
      row.OtherHDPTakeOverHint
    );
    const otherOuTakeOverTarget = selectOtherBettype(
      isEdited,
      row.OtherOUTakeOver,
      row.OtherOUTakeOverHint
    );

    if (row.InfoSiteMatchId > 0) {
      hights.push(calculateRowLevel(`${row.HomeName}${row.AwayName}`, 60));
    } else {
      hights.push(calculateRowLevel(row.LeagueName, 70));
      hights.push(calculateRowLevel(otherHdpTarget, 20, 1));
      hights.push(calculateRowLevel(otherOuTarget, 20, 1));
      hights.push(calculateRowLevel(otherHdpTakeOverTarget, 20, 1));
      hights.push(calculateRowLevel(otherOuTakeOverTarget, 20, 1));
      row.isInputTyping ? hights.push(2) : hights.push(1);
    }
    return mapRowLevelToHight(Math.max(...hights));
  }
  return 38;
};

const { rowHeight, maxHeight } = DefaultTableStyle;

const calculateTableHeight = dataLength =>
  dataLength > 27 ? maxHeight : rowHeight + dataLength * rowHeight + 20;

const {
  NoColumn,
  LeagueGroupColumn,
  LeagueColumn,
  IssuesColumn,
  HDPColumn,
  OUColumn,
  HDPTakeOverColumn,
  OUTakeOverColumn,
  MaxbetOUColumn,
  MaxbetHDPColumn,
  BlacklistColumn,
  NoteColumn,
  RemarkColumn,
  ActionColumn
} = ColumnModels;

const calculateColumnWidth = (isEditing, containerWidth) => {
  const ratio = containerWidth > 1750 ? containerWidth / 1800 : 1750 / 1800;
  const widthColumn = {
    LeagueGroup: LeagueGroupColumn.width * ratio,
    League: LeagueColumn.width * ratio,
    HDP: HDPColumn.width * ratio,
    Issues: IssuesColumn.width * ratio,
    OU: OUColumn.width * ratio,
    HDPTakeOver: HDPTakeOverColumn.width * ratio,
    OUTakeOver: OUTakeOverColumn.width * ratio,
    MaxBetHDP: MaxbetHDPColumn.width * ratio,
    MaxBetOU: MaxbetOUColumn.width * ratio,
    BlackList: BlacklistColumn.width * ratio,
    Note: NoteColumn.width * ratio,
    Remark: RemarkColumn.width * ratio
  };

  if (isEditing) {
    widthColumn.Action = ActionColumn.width * ratio;
  } else {
    widthColumn.No = NoColumn.width * ratio;
  }

  return widthColumn;
};

const calculateTableWidth = (column = {}) =>
  Object.values(column).reduce((x, y) => x + y, 0);

const formatMaxbet = maxbet => (maxbet ? `${maxbet / 1000}K` : '');
const formatSpread = spread => (spread ? `${(spread * 100).toFixed()}%` : '');

const getMaxbetSpreadValue = value => {
  const { Maxbet, Spread } = value;
  const maxbet = formatMaxbet(Maxbet);

  const spread =
    Maxbet && Spread ? `(${formatSpread(Spread)})` : formatSpread(Spread);
  return maxbet + spread;
};
export {
  calculateRowLevel,
  mapRowLevelToHight,
  calculateRowHeight,
  calculateTableHeight,
  calculateColumnWidth,
  calculateTableWidth,
  getMaxbetSpreadValue
};
