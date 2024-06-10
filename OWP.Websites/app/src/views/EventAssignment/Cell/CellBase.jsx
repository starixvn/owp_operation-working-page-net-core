import {
  MatchNo,
  KickOffTime,
  TV,
  Provider,
  MaxbetSpread,
  Remark
} from '../constants/Column';
import MatchNoCell from './MatchNoCell';
import KOTimeCell from './KOTimeCell';
import TVCell from './TVCell';
import ProviderCell from './ProviderCell';
import MaxbetSpreadCell from './MaxbetSpreadCell';
import RemarkCell from './RemarkCell';

const CellBase = (columnName, dataSource, rowIndex) => {
  switch (columnName) {
    case MatchNo.name:
      return <MatchNoCell data={dataSource} rowIndex={rowIndex} />;
    case KickOffTime.name:
      return <KOTimeCell data={dataSource} rowIndex={rowIndex} />;
    case TV.name:
      return <TVCell data={dataSource} rowIndex={rowIndex} />;
    case Provider.name:
      return <ProviderCell data={dataSource} rowIndex={rowIndex} />;
    case MaxbetSpread.name:
      return <MaxbetSpreadCell data={dataSource} rowIndex={rowIndex} />;
    case Remark.name:
      return <RemarkCell data={dataSource} rowIndex={rowIndex} />;
    default:
      return null;
  }
};

export default CellBase;
