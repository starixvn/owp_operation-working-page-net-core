import NoCell from './NoCell';
import LeagueGroupCell from './LeagueGroupCell';
import LeagueCell from './LeagueCell';
import MaxbetCell from './MaxbetCell';
import ActiveCell from './ActiveCell';
import LastUse from './LastUse';
import BlackListCell from './BlackListCell';
import ColumnConst from '../../../constants/ColumnConst';

const renderCellFactory = (dataSource, columnKey, rowIndex) => {
  const {
    no,
    leagueGroup,
    league,
    spread,
    hDPWeekday,
    hDPWeekend,
    fT1X2,
    active,
    blacklist,
    lastUse
  } = ColumnConst.columnMaxbetGeneral;
  switch (columnKey) {
    case no.columnKey:
      return <NoCell rowIndex={rowIndex} />;
    case leagueGroup.columnKey:
      return <LeagueGroupCell dataSource={dataSource} rowIndex={rowIndex} />;
    case league.columnKey:
      return <LeagueCell dataSource={dataSource} rowIndex={rowIndex} />;
    case spread.subHeader.min.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MinSpread} />;
    case spread.subHeader.max.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MaxSpread} />;
    case hDPWeekday.subHeader.min.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MinMaxbetWD} />;
    case hDPWeekday.subHeader.max.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MaxMaxbetWD} />;
    case hDPWeekend.subHeader.min.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MinMaxbetWK} />;
    case hDPWeekend.subHeader.max.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MaxMaxbetWK} />;
    case fT1X2.subHeader.min.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MinFT1X2} />;
    case fT1X2.subHeader.max.columnKey:
      return <MaxbetCell value={dataSource[rowIndex].MaxFT1X2} />;
    case active.columnKey:
      return <ActiveCell dataSource={dataSource} rowIndex={rowIndex} />;
    case blacklist.columnKey:
      return <BlackListCell dataSource={dataSource} rowIndex={rowIndex} />;
    case lastUse.columnKey:
      return <LastUse lastUseDate={dataSource[rowIndex].LastUse} />;
    default:
      return undefined;
  }
};

export default renderCellFactory;
