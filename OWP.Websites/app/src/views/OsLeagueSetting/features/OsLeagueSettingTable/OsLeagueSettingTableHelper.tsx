import {
  No,
  Category,
  League,
  MaxBetHDP,
  MaxBetOU,
  SpreadHDP,
  SpreadOU,
  Round,
  Summary,
  Action,
  DefaultTableStyle
} from './TableSchema.json';
import { Column, ColumnSchema, DataSource } from '../../typing/tableType';
import { dynamicCell } from './ConfigNative';

import { calculateCustomTableWidth } from '../../../Shared/common/CustomTable/Helper';

const buildColumn = (width: number, column: Column) => {
  const { headerName, columnKey } = column;
  return {
    headerName,
    columnKey,
    width
  };
};

const calculateRowHeight = (leagueOs: DataSource) => {
  if (!leagueOs) {
    return 0;
  }
  const { rowHeight, rowHeightPlus, MaximumLengthOfText } = DefaultTableStyle;
  const { summaryInfo, summaryRemark } = leagueOs;
  const infoText = summaryInfo.map(x => x.text).join(' ');
  const remarkText = summaryRemark.map(x => x.text).join(' ');

  if (
    infoText.length > MaximumLengthOfText ||
    remarkText.length > MaximumLengthOfText
  ) {
    return rowHeightPlus;
  }
  return rowHeight;
};

const selectTable = (widthWindow: number) => {
  const {
    minWidth,
    initialWidth,
    NoSize,
    CategorySize,
    LeagueSize,
    MaxBetHDPSize,
    MaxBetOUSize,
    SpreadHDPSize,
    SpreadOUSize,
    RoundSize,
    SummarySize,
    ActionSize
  } = DefaultTableStyle;
  const width = widthWindow < minWidth ? minWidth : widthWindow;

  return {
    columnSchema: [
      buildColumn(width * NoSize, No),
      buildColumn(width * CategorySize, Category),
      buildColumn(width * LeagueSize, League),
      buildColumn(width * MaxBetHDPSize, MaxBetHDP),
      buildColumn(width * MaxBetOUSize, MaxBetOU),
      buildColumn(width * SpreadHDPSize, SpreadHDP),
      buildColumn(width * SpreadOUSize, SpreadOU),
      buildColumn(width * RoundSize, Round),
      buildColumn(width * SummarySize, Summary),
      buildColumn(width * ActionSize, Action)
    ],
    tableStyle: DefaultTableStyle,
    dynamicCell,
    sortFunc: undefined,
    tableWidth: (columnSchema: ColumnSchema) =>
      calculateCustomTableWidth(columnSchema, initialWidth)
  };
};

export { selectTable, calculateRowHeight };
