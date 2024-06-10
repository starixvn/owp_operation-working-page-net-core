import renderGroupHeaderHasFilter from './renderGroupHeaderHasFilter';
import renderSingleHeaderHasFilter from './renderSingleHeaderHasFilter';
import renderSingleHeaderNoFilter from './renderSingleHeaderNoFilter';
import ColumnConst from '../../../constants/ColumnConst';

const renderTableBody = (dataSource, filterInfo) => {
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
  const column = [];
  column.push(renderSingleHeaderNoFilter([], no, true));
  column.push(
    renderSingleHeaderHasFilter(dataSource, filterInfo, leagueGroup, true)
  );
  column.push(
    renderSingleHeaderHasFilter(dataSource, filterInfo, league, true)
  );
  column.push(renderGroupHeaderHasFilter(dataSource, filterInfo, spread));
  column.push(renderGroupHeaderHasFilter(dataSource, filterInfo, hDPWeekday));
  column.push(renderGroupHeaderHasFilter(dataSource, filterInfo, hDPWeekend));
  column.push(renderGroupHeaderHasFilter(dataSource, filterInfo, fT1X2));
  column.push(renderSingleHeaderNoFilter(dataSource, active, false));
  column.push(renderSingleHeaderNoFilter(dataSource, blacklist, false));
  column.push(
    renderSingleHeaderHasFilter(dataSource, filterInfo, lastUse, false)
  );
  return column;
};

export default renderTableBody;
