import {
  NoCell,
  CategoryCell,
  RoundCell,
  SummaryCell,
  MaxBetCell,
  SpreadCell,
  ActionCell,
  LeagueCell
} from './OsLeagueSettingCell';
import {
  No,
  Category,
  League,
  MaxBet,
  Spread,
  Round,
  Summary,
  Action
} from './TableSchema.json';
import { CellModel } from '../../typing/tableType';

const dynamicCell = (rowIndex: number, cellModel: CellModel) => {
  const { dataSource, columnKey } = cellModel;
  switch (columnKey) {
    case No.columnKey:
      return <NoCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Category.columnKey:
      return <CategoryCell dataSource={dataSource} rowIndex={rowIndex} />;
    case League.columnKey:
      return <LeagueCell dataSource={dataSource} rowIndex={rowIndex} />;
    case MaxBet.columnKey:
      return <MaxBetCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Spread.columnKey:
      return <SpreadCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Round.columnKey:
      return <RoundCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Summary.columnKey:
      return <SummaryCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Action.columnKey:
      return <ActionCell dataSource={dataSource} rowIndex={rowIndex} />;
  }
  return null;
};

export { dynamicCell };
