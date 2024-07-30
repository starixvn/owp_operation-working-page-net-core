import {
  TraderCell,
  IPtvStreamingCell,
  MaxBetCell,
  ProviderCell,
  StatusCell,
  KOTimeCell,
  RemarkCell,
  EventCell,
  MatchCell,
  LeagueCell,
  SportCell,
  SpreadCell
} from './OSCell';
import {
  No,
  KOTime,
  Sport,
  Event,
  Match,
  League,
  Remark,
  Trader,
  MaxBet,
  Spread,
  IPTvStreaming,
  Provider
} from './TableSchema.json';

const dynamicCell = (rowIndex, cellModel) => {
  const { dataSource, columnKey, collapseExpandRowState, handleShowTicket } =
    cellModel;
  switch (columnKey) {
    case No.columnKey:
      return (
        <StatusCell
          dataSource={dataSource}
          rowIndex={rowIndex}
          handleShowDropdown={handleShowTicket}
          isLastRow={dataSource.length === rowIndex + 1}
        />
      );
    case KOTime.columnKey:
      return <KOTimeCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Sport.columnKey:
      return <SportCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Event.columnKey:
      return <EventCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Match.columnKey:
      return <MatchCell dataSource={dataSource} rowIndex={rowIndex} />;
    case League.columnKey:
      return <LeagueCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Remark.columnKey:
      return <RemarkCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Trader.columnKey:
      return (
        <TraderCell
          dataSource={dataSource}
          rowIndex={rowIndex}
          collapseExpandRowState={collapseExpandRowState}
        />
      );
    case MaxBet.columnKey:
      return <MaxBetCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Spread.columnKey:
      return <SpreadCell dataSource={dataSource} rowIndex={rowIndex} />;
    case IPTvStreaming.columnKey:
      return <IPtvStreamingCell dataSource={dataSource} rowIndex={rowIndex} />;
    case Provider.columnKey:
      return <ProviderCell dataSource={dataSource} rowIndex={rowIndex} />;
  }
  return null;
};

export { dynamicCell };
